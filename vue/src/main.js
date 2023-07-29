import Vue from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'
import vuetify from './plugins/vuetify'
import Filters from '@/lib/filters'
import Abp from './plugins/abp'
import money from 'v-money'
import Http from '@/lib/http'
import Util from '@/lib/util'
import PermissioNames from "@/store/permissioNames";
import { Vue2Storage } from 'vue2-storage'

import VueMask from 'v-mask'
Vue.use(VueMask);
Vue.use(Filters);
Vue.use(money)
Vue.prototype.$PermissioNames = PermissioNames
Vue.use(Abp)
Vue.use(Vue2Storage, {
    prefix: 'app_',
    driver: 'local',
    ttl: 60 * 60 * 24 * 1000 // 24 hours
})

Vue.config.productionTip = false

// new Vue({
//     router,
//     store,
//     vuetify,
//   render: function (h) { return h(App) }
// }).$mount('#app')

Http.get('/AbpUserConfiguration/GetAll').then((data) => {

    Util.abp = Util.extend(true, Util.abp, data.data.result)
    new Vue({
        router,
        store,
        vuetify,
        render: (h) => h(App),
        data: { currentPageName: '' },
        async mounted () {
            this.currentPageName = this.$route.name
            await this.$store.dispatch({
                type: 'Session/init',
            })
            this.$storage.setOptions({
                prefix: 'app_',
                driver: 'local',
                ttl: 60 * 60 * 24 * 1000 // 24 hours
            })
            this.$store.commit('App/initCachepage')
            this.$store.commit('App/updateMenulist')
        },
    }).$mount('#app')
})


