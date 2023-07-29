<template>
  <div class="d-flex flex-row justify-space-between  py-3">
      
    <div class="d-flex">
        
      <v-btn class="bbl-orange pr-6 hover-click mr-4" dark @click="adicionar" v-show="!occultAdd">
        <v-icon class="mr-1">mdi-plus</v-icon>Adicionar
      </v-btn>

        <v-btn class="bbl-blue pr-6 hover-click" dark @click="relatorio" v-show="!occultRelatorio">
            <v-icon class="mr-1">mdi-printer</v-icon>Relatório
        </v-btn>
    </div>

      <div
          class="d-flex flex-row flex-grow-1 align-center"
          style="max-width: 60%"
      >
          <v-text-field
              @click:clear="resetSearch"
              v-model="vModelSearch"
              @keypress="filtrarCaracterAlfanumericos($event)"
              clearable
              solo
              hide-details
              dense
              class="mx-2"
              :label="labelSearch"
              prepend-inner-icon="mdi-magnify"
              full-width
              @keyup.enter="emitSearch"
          ></v-text-field>
          <v-btn dark class="bbl-blue" @click="emitSearch">
              <v-icon>mdi-magnify</v-icon>
          </v-btn>
      </div>

   </div>
</template>

<script>
import { mapState, mapActions } from "vuex";
import ModalController from "@/mixins/ModalController";
import {filtrarCaracterAlfanumericos} from "@/lib/helpers/validarCaracteresPermitidos";

export default {
    name: 'AcoesListagem',
    props: {
        labelSearch: String,
        adicionar: {type: Function, default: () => {}},
        relatorio: {type: Function, default: () => {}},
        occultAdd: {type: Boolean, default: false},
        occultRelatorio: {type: Boolean, default: false},
    },

    data: () => ({
        vModelSearch: null,
    }),

    computed: {
        ...mapState(["filterIsOpen"]),
    },
    mixins: [
        ModalController
    ],
    watch: {
        
    },
    methods: {
        filtrarCaracterAlfanumericos,
        displayModal() {
            this.$emit("displayModal", "modalAdd");
        },
        selectOption(item) {
            this.$emit("optionSelected", item.value);
        },

        emitSearch() {
            this.$emit("search", this.vModelSearch);
        },

        resetSearch() {
            this.$emit("search", null);
        },

        ...mapActions(["handleFilter"]),

    },
};
</script>

<style>
</style>