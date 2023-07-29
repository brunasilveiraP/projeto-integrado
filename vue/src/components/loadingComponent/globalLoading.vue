<template>
  <div v-if="isLoading" class="loadingContainer">
    <v-progress-circular
      :size="130"
      :width="7"
      color="primary"
      indeterminate
    ></v-progress-circular>
  </div>
</template>

<script>
export default {
  data() {
    return {};
  },

  computed: {
    isLoading() {
      return this.$store.state.globalLoading;
    },
  },
};
</script>

<style lang="scss" scoped>
.loadingContainer {
  position: fixed;
  top: 0;
  left: 0;
  width: 100vw;
  height: 100vh;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 10000;
}
</style>
