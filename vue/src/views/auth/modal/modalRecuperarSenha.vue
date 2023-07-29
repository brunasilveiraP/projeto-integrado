<template>
  <v-dialog v-model="display" max-width="600px">
    <div class="white pa-8 d-flex flex-column">
      <div class="d-flex flex-row justify-space-between">
        <h2>Esqueci Minha Senha</h2>
        <v-btn text small fab @click="close">
          <v-icon>mdi-close</v-icon>
        </v-btn>
      </div>

      <p class="mt-3">
        Insira seu e-mail para solicitar a recuperação de senha. Após, verifique
        sua caixa de entrada para continuar.
      </p>

      <div class="my-4">
        <v-form ref="form" v-model="valid">
          <v-text-field
            type=""
            label="E-mail"
            class="mb-3"
            style="margin-top: -10px; font-size: 14px"
            v-model="inputData.email"
            :rules="[rules.required, rules.email]"
            @keypress.enter="submitForm"
            dense
            validate-on-blur
            clearable
          ></v-text-field>
        </v-form>
      </div>

      <div style="text-align: right">
        <v-btn class="bbl-orange white--text" @click="submitForm"
          >Enviar</v-btn
        >
      </div>
    </div>
  </v-dialog>
</template>

<script>
import ValidationRules from "@/mixins/validationRules.js";
import ModalController from "@/mixins/ModalController";
import {createNamespacedHelpers} from "vuex";

const moduleAuthenticate = createNamespacedHelpers("ModuleAuthenticate");

export default {
  data: () => ({
    valid: true,
    inputData: {},
    loading: false,
  }),

  methods: {
    submitForm() {
      if (this.$refs.form.validate()) {
        this.$emit("submit", this.inputData);
      }
    },
      
      close(){
          this.$emit("close");
      }
  },

  watch: {
    display(v) {
      if (v) this.inputData = {};
    },
  },

  props: ["display"],

  mixins: [ValidationRules, ModalController],
};
</script>

<style>
</style>