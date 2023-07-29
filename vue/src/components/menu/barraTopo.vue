<template>
    <v-app-bar
        dense
        app
        clipped-right
        flat
        height="60"
        class="elevation-0  bbl-neutral brighter"
    >
        <v-row>
            <v-btn
                class="flexcol hover-click mt-2 ml-4"
                fab
                icon
                medium
                on
                @click="$router.go(-1)"
            >
                <v-icon color="grey">mdi-arrow-left</v-icon>
                <span style="font-size: 10px">Voltar</span>
            </v-btn>

            <v-btn
                v-ripple="false"
                v-for="(item, i) in barraTopoItems"
                :key="i"
                class="flexcol hover-click mt-1 ml-6"
                text
                fab
                medium
                on
                @click="item.clickFunction()"
                :disabled="(item.hasDisabled && item.disabledFunction()) || disableIcons"
            >
                <v-icon :color="item.color">{{ item.icon }}</v-icon>
                <span style="font-size: 10px">{{ item.nome }}</span>
            </v-btn>

        </v-row>
    </v-app-bar>
</template>

<script>
import { mapActions } from "vuex";
export default {
    name: 'BarraTopo',
    data: () => ({
        show: true,
        disableIcons: true,
    }),
    methods: {
        ...mapActions(["updateTopBarItems"]),
    },
    computed: {
        barraTopoItems() {
            var icons = this.$store.state.barraTopoItems;
            if (icons === "close") this.disableIcons = true;
            else this.disableIcons = false;
            return icons;
        },
    },
    mounted() {
        this.$route.name === "ViewAdministracao"
            ? (this.show = true)
            : (this.show = false);
    },
    watch: {
        $route(to, from) {
            to.name === "ViewAdministracao" ? this.show = true : this.show = false;
            this.updateTopBarItems([]);
        },
    },
}
</script>

<style lang="scss">
.flexcol .v-btn__content {
    flex-direction: column;
}

.v-btn::before {
    background-color: transparent;
}

.hover-click:hover {
    transition: all 200ms ease-in;
    transform: scale(1.1);
    cursor: pointer !important;
    -webkit-transition: all 200ms ease-in;
    -webkit-transform: scale(1.1);
    -ms-transition: all 200ms ease-in;
    -ms-transform: scale(1.1);
    -moz-transition: all 200ms ease-in;
    -moz-transform: scale(1.1);
}
.v-app-bar{
    color: #2C7BA1;
}
</style>