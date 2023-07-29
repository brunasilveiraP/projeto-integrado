<template>
  <v-dialog
    v-model="display"
    max-width="400px"
    persistent
    @click:outside="close"
  >
    <div class="white pa-6 d-flex flex-column align-center text-center">
      <v-icon style="font-size: 60px" color="warning">mdi-alert</v-icon>
      <h3 class="aapecan-neutral--text text--var-1 pb-5 pt-5">Excluir Registro</h3>
      <div>
        <p class="pb-3">O registro será excluído do sistema. Deseja continuar?</p>
        <div class="d-flex flex-row justify-center">
          <v-btn text @click="close()">Cancelar</v-btn>
          <v-btn
            class="warning aapecan-neutral--text elevation-0 ml-4"
            @click="confirmDelete()"
            >Excluir</v-btn
          >
        </div>
      </div>
    </div>
  </v-dialog>
</template>
  
  <script>
export default {
  data: () => ({
    display: false,
    itemId: null,
  }),

  methods: {
    confirmDelete() {
      this.$emit("isComfirmDeleteItem", this.itemId);
      this.close();
    },

    open(itemId) {
      this.itemId = itemId;
      this.display = true;
    },

    close() {
      this.display = false;
      this.itemId = null;
    },
  },
};
</script>
  <style scoped>
</style>