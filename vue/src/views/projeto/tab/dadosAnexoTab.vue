<template>
    <div class="flex-grow-1">

        <v-alert
            dismissible
            :type="alertType"
            width="40%"
            style="position: absolute; top: 10%; right: 1%"
            v-show="alertShow"
            transition="scroll-x-reverse-transition"
        >
            {{ alertText }}
        </v-alert>

        <modal-delete
            ref="deleteItemRef"
            @isComfirmDeleteItem="confirmDelete($event)"
        />

        <v-form ref="form" v-model="valid">
            <div class="d-flex mx-4">
                <div
                    class="my-4 mr-4 uploadArea flex-grow-1 rounded-lg elevation-3"
                    @drop.prevent="onDrop($event)"
                    @dragover.prevent="dragover = true"
                    @dragenter.prevent="dragover = true"
                    @dragleave.prevent="dragover = false"
                    :class="{ 'grey lighten-2': dragover }"
                >
                    <v-row>
                        <v-col :cols="uploadedFiles.length > 0 ? 6 : 12">
                            <div :class="uploadedFiles.length > 0 ? 'mt-10 ml-2' : 'mt-6'">
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
                                    accept=".jpg, .jpeg, .png, .pdf"
                                    v-on:change="uploadAnexo($event)"
                                />
                                <p style="text-align: center">
                    <span class="attachmentsText"
                    >Formatos permitidos: JPEG, JPG, PDF e PNG.</span
                    ><br />
                                    <span class="attachmentsText">Tamanho máximo de 30MB.</span
                                    ><br />
                                </p>
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
                          - {{ arquivo.name | truncate(26) }}
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
                <div
                    class="my-4 flex-grow-1 rounded-lg white elevation-3"
                    style="width: 40%; height: 200px"
                >
                    <div class="pa-4">
                        <div class="containerTexto">
                            <v-select
                                label="Tipo de Anexo"
                                item-value="id"
                                item-text="nome"
                                v-model="tipoAnexo"
                                :rules="[rules.required]"
                                class="required"
                                style="width: 100%"
                                :items="tipoAnexoEnum"
                                :menu-props="{ bottom: true, offsetY: true }"
                                clearable
                            ></v-select>
                        </div>

                    </div>
                </div>
            </div>

            <div class="mb-5 mr-4" style="text-align: right">
                <v-btn class="bbl-orange white--text" @click="submitForm" :loading="loading" :disabled="loading"
                >Salvar
                </v-btn>
            </div>
        </v-form>

        <div class="d-flex mb-9">
            <div
                class="
            d-flex
            flex-column
            mx-4
            pa-4
            flex-grow-1
            rounded-lg
            white
            elevation-3
          "
            >
                <!--Datatable-->
                <v-data-table
                    :key="key"
                    :headers="headers"
                    :items="rows"
                    noDataText="Nenhum resultado encontrado."
                    :loading="tableLoading"
                    loading-text="Aguarde, carregando anexos..."
                    style="width: 100%"
                    hide-default-footer
                    :header-props="{sortIcon: null}"
                >
                    <!-- Icone Manutenção -->
                    <template v-slot:[`item.col1`]="{ item }">
                        <v-menu bottom left>
                            <template v-slot:activator="{ on, attrs }">
                                <v-btn
                                    v-bind="attrs"
                                    v-on="on"
                                    icon
                                    @click="openDeleteModal(item.col1)"
                                >
                                    <v-icon> mdi-delete </v-icon>
                                </v-btn>
                            </template>
                        </v-menu>
                    </template>

                    <template v-slot:[`item.col2`]="{ item }">
              <span
                  @click="downloadAnexo(item)"
                  class="hover-click"
                  style="text-decoration: underline"
              >{{ item.col2 }}</span
              >
                    </template>
                </v-data-table>
            </div>
        </div>
    </div>
</template>

<script>
import validationRules from "@/mixins/validationRules.js";
import { toBase64 } from "@/lib/helpers/base64";
import modalDelete from "@/components/shared/modalDelete.vue";
import { mapActions, createNamespacedHelpers } from "vuex";
const moduleTipoAnexo = createNamespacedHelpers("ModuleTipoAnexo");
const moduleProjeto = createNamespacedHelpers("ModuleProjeto");
const moduleAnexo = createNamespacedHelpers("ModuleAnexo");

export default {
    components: {  modalDelete},
    props: {
        multiple: {
            type: Boolean,
            default: false,
        },
        projetoId: Number,
    },

    data: () => ({
        loading: false,
        valid: true,
        editId: 0,
        trigger: 0,
        inputData: {
            nomeArquivo: null,
            nrquivo: null,
            tipoAnexoId: null,
            projetoId: null,
            tamanhoArquivo: null,
            tipoArquivo: null,
        },
        tipoAnexoEnum: [],
        carregandoAnexos: false,
        displayModalAnexo: false,
        dragover: false,
        uploadedFiles: [],
        tableLoading: true,
        headers: [
            { text: "", value: "col1" },
            { text: "Anexo", value: "col2" },
            { text: "Tipo de Anexo", value: "col3" },
        ],
        rows: [],
        key: 1,
        alertType: null,
        alertText: '',
        alertShow: false,
    }),

    computed: {
        tipoAnexo: {
            get() {
                return +this.inputData.tipoAnexoId
            },
            set(newValue) {
                this.inputData.tipoAnexoId = newValue;
            }
        }
    },
    methods: {
        ...moduleTipoAnexo.mapActions(["getAllTiposAnexo"]),
        ...moduleAnexo.mapActions(["delete", "create", "getAllAnexosByProjeto"]),

        async getTiposAnexo(){
            this.tipoAnexoEnum = await this.getAllTiposAnexo();
        },

        deletarAnexo(index) {
            this.uploadedFiles.splice(index, 1);
        },

        async confirmDelete(itemId) {
            const isDeleted = await this.delete(itemId);
            if (isDeleted.status === 200) {
                this.mensagemSucesso("Anexo excluído com sucesso!")
                await this.loadAnexos();
            }else{
                this.mensagemErro("Ocorreu um erro ao realizar esta ação. Contate o administrador do sistema.");
            }
        },

        async openDeleteModal(itemId) {
            this.$refs.deleteItemRef.open(itemId);
        },

        async loadAnexos(){
            this.tableLoading = true;
            var data = await this.getAllAnexosByProjeto(this.projetoId);
            this.buildRows(data)
            this.tableLoading = false;
        },

        buildRows(data) {
            let dataRows = [];
            data.forEach((e) => {
                dataRows.push({
                    col1: e.id,
                    col2:
                        e.nomeArquivo +
                        "." +
                        e.tipoArquivo
                            .slice(e.tipoArquivo.indexOf("/") + 1, e.tipoArquivo.length)
                            .toUpperCase(),
                    col3: this.tipoAnexoFilter(e.tipoAnexo),
                    col4: e.arquivo,
                    col5: e.tipoArquivo,
                });
            });
            this.rows = dataRows;
        },

        downloadAnexo(file) {
            var a = document.createElement("a");
            a.href = "data:" + file.col5 + ";base64," + file.col4;
            a.download = file.col2;
            a.click();
        },

        tipoAnexoFilter(value) {
            let tipoAnexo = null;
            this.tipoAnexoEnum.map((x) => {
                if (x.id == value.id) tipoAnexo = x.nome;
            });
            return tipoAnexo;
        },
        async submitForm() {
            if (this.uploadedFiles.length > 5) {
                this.mensagemErro("Carregar no máximo 5 arquivos de cada vez.");
                return
            }
            if (this.uploadedFiles.length > 0) {
                this.loading = true;
                if (this.$refs.form.validate()) {
                    for (var i = 0; i < this.uploadedFiles.length; i++) {
                        var file = await toBase64(this.uploadedFiles[i], this.asText);
                        this.inputData.nomeArquivo = this.uploadedFiles[i].name.slice(0, this.uploadedFiles[i].name.indexOf("."));
                        this.inputData.arquivo = file;
                        this.inputData.projetoId= this.$route.params.id;
                        this.inputData.tamanhoArquivo= this.uploadedFiles[i].size;
                        this.inputData.tipoArquivo= this.uploadedFiles[i].type;

                        let response = await this.create(this.inputData);

                        if (response.status === 200) {
                            this.key++;
                            this.mensagemSucesso("Arquivo(s) incluídos com sucesso");
                            this.loadAnexos();
                        } else {
                            this.mensagemErro("Ocorreu um erro...");
                        }
                        this.loading = false;
                    }
                    this.resetData();
                }
                this.loading = false;
            } else {
                this.mensagemErro("Nenhum arquivo foi anexado...");
            }
        },
        resetData() {
            this.inputData.tipoAnexo = 0;
            this.uploadedFiles = [];
            this.$refs.form.resetValidation();
        },
        verificarExtencaoArquivosUpload(arquivo) {
            const name = arquivo.split('.')
            if(name[1] === 'pdf')
                return true;
            if(name[1] === 'jpeg')
                return true
            if(name[1] === 'jpg')
                return true
            if(name[1] === 'png')
                return true
            return false
        },

        onDrop(e) {
            const isValid = this.verificarExtencaoArquivosUpload(e.dataTransfer.files[0].name)
            if(isValid) {
                this.carregandoAnexos = true;
                this.dragover = false;
                if (this.uploadedFiles.length > 0) this.uploadedFiles = [];
                if (e.dataTransfer.files.length >= 1) {
                    var files = e.dataTransfer.files;
                    for (var i = 0; i < files.length; i++) {
                        this.validarAnexos(files[i]);
                    }
                }
                this.carregandoAnexos = false;
            }
            else {
                this.dragover = false;
                this.$emit("alert-error", {
                    alertError: true,
                    alertErrorText: "Formato do arquivo não suportado.",
                });
            }
        },
        async uploadAnexo(e) {
            this.carregandoAnexos = true;
            if (e.target.files.length > 0) {
                this.uploadedFiles.push(e.target.files[0]);
            }
            this.carregandoAnexos = false;
        },
        async validarAnexos(file) {
            var extensao = [".JPG", ".PNG", ".JPEG", ".PDF"];
            var maxFileSize = 31457280;
            var extensaoArquivo = file.name.substr(
                file.name.indexOf("."),
                file.name.length
            );
            if (
                extensao.includes(extensaoArquivo.toUpperCase()) &&
                file.size <= maxFileSize
            )
                this.uploadedFiles.push(file);
        },

        mensagemSucesso(mensagem){
            this.alertShow = true;
            this.alertType = "success";
            this.alertText = mensagem;
            setTimeout(() => {
                this.alertShow = false;
            }, 5000);
        },

        mensagemErro(mensagem){
            this.alertShow = true;
            this.alertType = "error";
            this.alertText = mensagem;
            setTimeout(() => {
                this.alertShow = false;
            }, 5000);
        }
    },

    mounted() {
        this.aditionalData = this.$route.params.id;
        this.getTiposAnexo();
        this.loadAnexos();
    },
    mixins: [
        validationRules,
    ],
};
</script>

<style scoped>
.containerTexto {
    display: flex;
    position: relative;
    justify-content: left;
}

.textarea {
    position: relative;
    align-items: center;
    justify-content: center;
}

.textarea >>> .v-label {
    height: auto;
    top: 25px !important;
}

.texto {
    display: flex;
    justify-content: center;
    align-items: center;
    gap: 5px;
}

.v-application p {
    margin-bottom: 0px !important;
}
#upload {
    display: none;
}
.mensagem:focus {
    background: #ffff;
}
.uploadIcon {
    left: 10%;
    top: 50px;
    bottom: 50px;
}
.uploadArea {
    width: 50%;
    height: 200px;
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
    height: 180px;
    margin: 10px 10px 10px 10px;
}
</style>
  