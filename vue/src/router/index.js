import Vue from 'vue'
import VueRouter from 'vue-router'
import ViewGlobal from '@/views/baseView/ViewGlobal'
import ViewAdministracao from '@/views/administracao/ViewAdministracao'
import ViewTabelaPreco from '@/views/tabelaPreco/ViewTabelaPreco'
import Authenticate from '@/views/auth/authenticate.vue'
import ViewConcessionaria from "@/views/concessionaria/ViewConcessionaria";
import ViewFase from "@/views/fase/ViewFase";
import ViewTipoAnexo from "@/views/tipoAnexo/ViewTipoAnexo";
import ViewUsuario from "@/views/usuario/ViewUsuario";
import ViewParceiro from "@/views/parceiro/ViewParceiro";
import ViewProjeto from "@/views/projeto/ViewProjeto";
import ViewManutencaoParceiro from "@/views/parceiro/ViewManutencaoParceiro";
import ViewManutencaoCobranca from "@/views/cobranca/ViewManutencaoCobranca";
import ViewCobranca from "@/views/cobranca/ViewCobranca";
import ViewManutencaoProjeto from "@/views/projeto/ViewManutencaoProjeto";

Vue.use(VueRouter)

export const appRouters = [
]

const routes = [
    {
        path: '/',
        name: 'ViewAuthenticate',
        component: Authenticate,
    },
    {
        path: '/app',
        name: 'ViewGlobal',
        component: ViewGlobal,
        children: [
            {
                path: '/app/administracao',
                name: 'ViewAdministracao',
                component: ViewAdministracao,
            },
            {
                path: '/app/tabelaPreco',
                name: 'ViewTabelaPreco',
                component: ViewTabelaPreco,
            },
            {
                path: '/app/concessionaria',
                name: 'ViewConcessionaria',
                component: ViewConcessionaria,
            },
            {
                path: '/app/fase',
                name: 'ViewFase',
                component: ViewFase,
            },
            {
                path: '/app/tipoAnexo',
                name: 'ViewTipoAnexo',
                component: ViewTipoAnexo,
            },
            {
                path: '/app/usuario',
                name: 'ViewUsuario',
                component: ViewUsuario,
            },
            {
                path: '/app/parceiro',
                name: 'ViewParceiro',
                component: ViewParceiro,
            },
            {
                path: '/app/parceiro/editar/:id',
                name: 'ViewManutencaoParceiroEditar',
                props: true,
                params:true,
                component: ViewManutencaoParceiro,
            },
            {
                path: '/app/parceiro/adicionar/',
                name: 'ViewManutencaoParceiroAdicionar',
                props: false,
                params:false,
                component: ViewManutencaoParceiro,
            },
            {
                path: '/app/cobranca/',
                name: 'ViewCobranca',
                component: ViewCobranca,
            },
            {
                path: '/app/cobranca/editar/:id',
                name: 'ViewManutencaoCobrancaEditar',
                props: true,
                params:true,
                component: ViewManutencaoCobranca,
            },
            {
                path: '/app/projeto',
                name: 'ViewProjeto',
                component: ViewProjeto,
            },
            {
                path: '/app/projeto/editar/:id',
                name: 'ViewManutencaoProjetoEditar',
                props: true,
                params:true,
                component: ViewManutencaoProjeto,
            },
            
        ]
    },
    //{
    //path: '/about',
    //name: 'about',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    //component: function () {
    //return import(/* webpackChunkName: "about" */ '../views/AboutView.vue')
    //}
    //}
]

const router = new VueRouter({
    mode: 'hash',
    base: process.env.BASE_URL,
    routes
})

export default router
