<template>
    <v-alert
        dismissible
        v-show="alertStore"
        :type="type"
        class="alertaContainer mt-2"
        transition="slide-x-reverse-transition"
    >
        {{ message }}
    </v-alert>
</template>
<script>

export default {
    data() {
        return {
            display: false,
            type: null,
            message: null
        }
    },
    computed: {
        alertStore() {
            this.message = this.$store.state.ModuleAlert.message;
            this.type = this.$store.state.ModuleAlert.type;
            return this.$store.state.ModuleAlert.displayAlert;
        },
    },
};
</script>

<style lang="scss" scoped>
.alertaContainer {
    position: absolute;
    top: 60px;
    right: 30px;
    width: 25vw;
    z-index: 1000;
}
</style>