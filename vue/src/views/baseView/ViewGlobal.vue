<template>
    <div>
        <barra-topo/>
        <menu-lateral/>
        <v-main class=" bbl-neutral brighter conteudo">
            <router-view
                class="pa-0 mt-0 ma-0 px-4"
                v-slot="{ Component }"
            >
                <transition name="fade" mode="out-in">
                    <component :is="Component" />
                </transition>
            </router-view>
        </v-main>
    </div>

    <!--    <div class="d-flex flex-row bbl-orange-banner brighter" style="height: 100%">-->
    <!--        <menu-lateral/>-->
    <!--        <div class="d-flex flex-column flex-grow-1 wrapperContainer">-->
    <!--            <barra-topo></barra-topo>-->
    <!--            <div class="conteudo">-->
    <!--                <div class="container-limit">-->
    <!--                    <transition mode="out-in">-->
    <!--                        <router-view :key="$route.path"></router-view>-->
    <!--                    </transition>-->
    <!--                </div>-->
    <!--            </div>-->
    <!--        </div>-->
    <!--    </div>-->
</template>

<script>

import MenuLateral from '@/components/menu/menuLateral'
import BarraTopo from '@/components/menu/barraTopo'

import {mapState} from 'vuex'

export default {
    name: 'ViewGlobal',
    components: {
        MenuLateral,
        BarraTopo
    },
}
</script>

<style lang="scss" scoped>
.conteudo {
    width: 100%;
    height: calc(100vh - 1px);
}
.wrapperContainer {
    width: calc(100vw - 70px);

    &.active {
        width: calc(100vw - 270px);
    }
}
.container-limit {
    width: 100%;
    max-width: 1900px;
}
</style>