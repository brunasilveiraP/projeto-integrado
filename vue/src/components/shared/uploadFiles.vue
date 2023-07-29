<template>
  <div
    class="my-4 uploadArea flex-grow-1 rounded-lg elevation-6"
    @drop.prevent="onDrop($event)"
    @dragover.prevent="dragover = true"
    @dragenter.prevent="dragover = true"
    @dragleave.prevent="dragover = false"
    :class="{ 'grey lighten-2': dragover }"
  >
    <v-row>
      <v-col :cols="uploadedFiles.length > 0 ? 6 : 12">
        <div :class="uploadedFiles.length > 0 ? 'mt-10 ml-2' : 'mt-6'">
          <div v-if="uploadedFiles.length == 0">
            <v-icon
              x-large
              :style="
                uploadedFiles.length > 0 ? 'margin:-125px 0px 0px 90px' : ''
              "
              class="aapecan-purple--text uploadIcon"
              >mdi-cloud-upload</v-icon
            >
            <p style="text-align: center">
              Arraste e largue seu(s) arquivo(s) aqui para anexar <br />ou
              <span
                @click="$refs.file.click()"
                class="aapecan-purple--text hover-click"
                style="text-decoration: underline"
                >localize em seu computador</span
              >
            </p>
            <input
              id="upload"
              type="file"
              ref="file"
              :accept="['.csv']"
              v-on:change="uploadAnexo($event)"
            />
            <p style="text-align: center">
              <span class="attachmentsText">Formatos permitidos: CSV.</span
              ><br />
              <span class="attachmentsText">Tamanho máximo de 200MB.</span
              ><br />
            </p>
          </div>
          <div v-else>
            <v-col>
              <v-row justify="center">
                <v-icon x-large class="aapecan-purple--text mt-n2 mb-2"
                  >mdi-cloud-upload</v-icon
                >
              </v-row>
              <v-row justify="center">
                <p style="text-align: center">
                  Arraste e largue seu(s) arquivo(s) aqui para anexar <br />ou
                  <span
                    @click="$refs.file.click()"
                    class="aapecan-purple--text hover-click"
                    style="text-decoration: underline"
                    >localize em seu computador</span
                  >
                </p>
                <input
                  id="upload"
                  type="file"
                  ref="file"
                  :accept="['.csv']"
                  v-on:change="uploadAnexo($event)"
                />
                <p style="text-align: center">
                  <span class="attachmentsText">Formatos permitidos: CSV.</span
                  ><br />
                  <span class="attachmentsText">Tamanho máximo de 200MB.</span
                  ><br />
                </p>
              </v-row>
            </v-col>
          </div>
        </div>
      </v-col>
      <v-col v-if="uploadedFiles.length > 0" cols="6">
        <v-card
          :loading="carregandoAnexos"
          class="form-uploader"
          style="overflow-y: auto; overflow-x: hidden"
        >
          <template>
            <v-row
              class="mt-n2 mb-n6 ml-n2"
              v-for="(arquivo, index) in uploadedFiles"
              :key="index"
            >
              <v-col>
                <span class="attachmentsList" color="black">
                  <v-icon color="grey" small> mdi-paperclip </v-icon>
                  - {{ arquivo.name | truncate(truncateValue) }}
                </span>
                <v-tooltip open-delay="500" bottom>
                  <template v-slot:activator="{ on, attrs }">
                    <v-btn-toggle group style="padding-left: 5px">
                      <span v-on="on" v-bind="attrs">
                        <v-icon
                          v-bind="attrs"
                          v-on="on"
                          small
                          @click="deletarAnexo(index)"
                          color="primary"
                        >
                          mdi-trash-can-outline
                        </v-icon>
                      </span>
                    </v-btn-toggle>
                  </template>
                  <span>Excluir</span>
                </v-tooltip>
              </v-col>
            </v-row>
          </template>
        </v-card>
      </v-col>
    </v-row>
  </div>
</template>

<script>
import validationRules from "@/mixins/validationRules.js";
import handleModal from "@/mixins/handleModal.js";
import searchFilters from "@/mixins/searchFilters.js";
import { mapActions, createNamespacedHelpers } from "vuex";

export default {
  props: {
    uploadedFiles: [],
    truncateValue: {
      type: [Number],
      default: 26,
    },
    multiple: {
      type: [Boolean],
      default: false,
    },
  },

  data: () => ({
    carregandoAnexos: false,
    dragover: false,
  }),

  methods: {
    emitErrorMultiple() {
      this.$emit(
        "onError",
        "Não é permitido mais do que um arquivo por importação"
      );
    },
    emitErrorExtension() {
      this.$emit(
        "onError",
        this.multiple
          ? "Um ou mais arquivos selecionados não estão na extensão correta"
          : "O arquivo selecionado não está na extensão correta"
      );
    },
    emitErrorLimite() {
      this.$emit(
        "onError",
        this.multiple
          ? "Um ou mais arquivos selecionados ultrapassam o limite de tamanho"
          : "O arquivo selecionado ultrapassa o limite de tamanho"
      );
    },
    deletarAnexo(index) {
      this.uploadedFiles.splice(index, 1);
    },
    onDrop(e) {
      this.carregandoAnexos = true;
      this.dragover = false;
      if (this.uploadedFiles.length > 0) this.uploadedFiles = [];
      if (!this.multiple && e.dataTransfer.files.length > 1) {
        this.emitErrorMultiple();
      } else if (e.dataTransfer.files.length >= 1) {
        let showExtensionError = false;
        var files = e.dataTransfer.files;
        for (var i = 0; i < files.length; i++) {
          this.validarAnexos(files[i]);
        }
        if (showExtensionError) this.emitErrorExtension();
      }
      this.carregandoAnexos = false;
    },
    async uploadAnexo(e) {
      this.carregandoAnexos = true;
      if (!this.multiple && this.uploadedFiles.length >= 1) {
        this.emitErrorMultiple();
      } else if (e.target.files.length > 0) {
        this.validarAnexos(e.target.files[0]);
      }
      this.carregandoAnexos = false;
    },
    async validarAnexos(file) {
      var extensao = [".CSV"];
      var maxFileSize = 209715200;
      var extensaoArquivo = file.name.substr(
        file.name.indexOf("."),
        file.name.length
      );
      if (!extensao.includes(extensaoArquivo.toUpperCase()))
        this.emitErrorExtension();
      else if (!file.size > maxFileSize) this.emitErrorSize();
      else this.uploadedFiles.push(file);
    },
  },

  mixins: [validationRules, handleModal, searchFilters],
};
</script>

<style scoped>
#upload {
  display: none;
}
.uploadIcon {
  left: 10%;
  top: 50px;
  bottom: 50px;
}
.uploadArea {
  width: 50%;
  height: 220px;
  justify-content: center;
  align-items: center;
  background: #f5f5f5;
}
.attachmentsText {
  font-family: "Open Sans";
  font-style: normal;
  font-weight: 400;
  font-size: 13px;
  line-height: 18px;
  text-align: center;
  color: #cccccc;
}
.attachmentsList {
  font-family: "Open Sans";
  font-style: normal;
  font-weight: 400;
  font-size: 12px;
  line-height: 17px;
  text-align: center;
  color: #585656;
}
.hover-click:hover {
  cursor: pointer !important;
}
.form-uploader {
  height: 200px;
  margin: 10px 10px 10px 10px;
}
</style>
