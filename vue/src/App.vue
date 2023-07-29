<template>
    <v-app>
        <Alert></Alert>
        <Loading></Loading>
        <transition mode="out-in">
            <router-view></router-view>
        </transition>
    </v-app>
</template>

<script>
import Alert from '@/components/shared/alertComponent.vue'
import Loading from '@/components/loadingComponent/globalLoading.vue'
export default {
    name: 'App',
    components: {
        Alert,
        Loading,
    },
    data: () => ({

    }),
};

</script>

<style lang="scss">
.v-enter,
.v-leave-to {
    transform: translate3d(-5px, 0, 0);
    opacity: 0;
}

.v-enter-active,
.v-leave-active {
    transition: all 0.3s;
}

::-webkit-scrollbar {
    width: 8px;
}

::-webkit-scrollbar-track {
    border-radius: 1px;
}

::-webkit-scrollbar-thumb {
    background: #FF6A13;
    border-radius: 1px;
}

::-webkit-scrollbar-thumb:hover {
    background: #E46419;
}

input[type=text]{
    
}

</style>
