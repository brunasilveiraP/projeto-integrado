import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex)

import App from "/src/store/app"
import Session from '@/store/session.js'
import ModuleAlert from '@/store/moduleAlert'
import ModuleAuthenticate from '@/store/moduleAuthenticate'
import ModuleTabelaPreco from '@/store/moduleTabelaPreco'
import Notification from '@/store/notifications.js'
import Money from "@/components/shared/Money";
import ModuleConcessionaria from "@/store/moduleConcessionaria";
import ModuleFase from "@/store/moduleFase";
import ModuleTipoAnexo from "@/store/moduleTipoAnexo";
import ModuleUsuario from "@/store/moduleUsuario";
import ModuleRoles from "@/store/moduleRoles";
import ModuleParceiro from "@/store/moduleParceiro";
import ModuleProjeto from "@/store/moduleProjeto";
import ModuleCobranca from "@/store/moduleCobranca";
import ModuleAnexo from "@/store/moduleAnexo";
import ModuleProjetoObservacao from "@/store/moduleProjetoObservacao";

export default new Vuex.Store({
    
  state: {
      globalLoading: false,
      filterIsOpen: false,
      barraTopoItems: []
  },
  getters: {
  },
  mutations: {
      OPEN_FILTER(state){
          state.filterIsOpen = true
      },

      CLOSE_FILTER(state){
          state.filterIsOpen = false
      }
  },
  actions: {
      changeGlobalLoading(context, payload){
          context.state.globalLoading = payload
      },
      updateTopBarItems(context, payload){
          context.state.barraTopoItems = payload
      },
      handleFilter(context, payload){

          if(payload == 'open'){
              context.commit('OPEN_FILTER')
          }

          if(payload == 'close'){
              context.commit('CLOSE_FILTER')
          }

      }
  },
  modules: {
      App,
      Session,
      Notification,
      ModuleAlert,
      ModuleAuthenticate,
      ModuleTabelaPreco,
      ModuleConcessionaria,
      ModuleFase,
      ModuleTipoAnexo,
      ModuleUsuario,
      ModuleRoles,
      ModuleParceiro,
      ModuleProjeto,
      ModuleCobranca,
      ModuleAnexo,
      ModuleProjetoObservacao,
      Money,
  }
})
