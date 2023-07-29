import Vue from 'vue'
import Vuetify from 'vuetify/lib'
import pt from 'vuetify/es5/locale/pt'

Vue.use(Vuetify)

export default new Vuetify({
    theme: {
        themes: {
            light: {
                primary: '#FF6A13',
                lighten: '#ff8843',
                secondary: '#103B4D',
                success: '#107154',
                error: '#C90C0F',
                warning: '#FECC00',
                info: '#2196F3'
            },
        },
    },
    lang: {
        locales: { pt },
        current: 'pt',
    },
    icons: {
        iconfont: 'mdi', // default - only for display purposes
    },
})
