<template>
     <v-dialog
      v-model="value"
      max-width="350px"
      persistent
      @click:outside="close"
    >
      <v-card class="white pa-6 d-flex flex-column align-center text-center">
        <v-icon style="font-size: 60px" color="warning" class="py-5">mdi-alert</v-icon>
        <p class="pb-3">
          Você está prestes a resetar a senha deste usuário. Esta ação não poderá ser desfeita. Deseja continuar?
        </p>
        <div class="d-flex flex-row justify-center">
          <v-btn class="px-5 font-weight-bold" text @click="close">CANCELAR</v-btn>

          <v-btn
            class="warning bbl-neutral--text elevation-0 ml-4 px-5 font-weight-bold"
            @click="confirmRecuperacaoSenha()"
            >CONTINUAR</v-btn
          >
        </div>
      </v-card>
    </v-dialog>
  </template>
    
    <script>
    import { mapActions } from "vuex";
    import ModalController from "@/mixins/ModalController";
  export default {
    mixins: [ ModalController ],
      props:{usuarioId : Number},
    data: () => ({
      display: false,
      itemId: null,
      email: null
    }),
  
    methods: {
        ...mapActions("ModuleAuthenticate" ,["internalPasswordRecovery", "externalPasswordRecovery"]),
      confirmRecuperacaoSenha() {
        if(this.usuarioId != null) this.handleRecuperarSenhaPorId()
        if(this.email != null) this.handleRecuperarSenhaPorEmail()
        this.close()
      },

      recuperarSenhaPorId(id) {
        this.usuarioId = id;
        this.display = true;
      },
      async handleRecuperarSenhaPorId() {
        const response = await this.internalPasswordRecovery({id: this.usuarioId});
        if(response.status === 200){
            this.$emit("dispararMensagemSucesso", "E-mail de recuperação de senha encaminhado com sucesso!");
        }else{
            this.$emit("dispararMensagemErro", response.error.message);
        }
        
        this.close()
      },

      recuperarSenhaPorEmail(email) {
        this.email = email
        this.display = true;
      },
      async handleRecuperarSenhaPorEmail() {
        const response = await this.externalPasswordRecovery(this.email);
        this.$emit("dispararMensagemSucesso", "E-mail de recuperação de senha encaminhado com sucesso!");
        this.close()
      },
  
      close() {
          this.$emit("input", false);
        this.itemId = null;
        this.email = null;
      },
    
    },
  };
  </script>
    <style scoped>
  </style>