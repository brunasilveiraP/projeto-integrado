export default {
  namespaced: true,
  state: {
    type: 'info',
    message: null,
    details: null,
    errors: [],
    visible: false,
  },
  getters: {
    visible: (state) => state.visible,
  },
  actions: {
    notify (context, payload) {
      context.commit('setNotification', payload.data)
      setTimeout(() => context.commit('clear'), 4500)
    },
    notifyAddSuccess (context) {
      context.commit('setNotification', { message: 'Item adicionado com sucesso', type: 'success' })
      setTimeout(() => context.commit('clear'), 4500)
    },
    notifyDeleteSuccess (context) {
      context.commit('setNotification', { message: 'Item excluído com sucesso', type: 'success' })
      setTimeout(() => context.commit('clear'), 4500)
    },
    notifyErrors (context, payload) {
      context.commit('setErrors', payload.data)
      setTimeout(() => context.commit('clear'), 4500)
    },
    notifyInfo (context, message) {
      context.commit('setNotification', { message: message, type: 'info' })
      setTimeout(() => context.commit('clear'), 4500)
    },
  },
  mutations: {
    clear (state) {
      state.type = 'info'
      state.errors = null
      state.message = null
      state.details = null
      state.visible = false
    },
    setNotification (state, notification) {
      state.errors = null
      state.type = notification.type
      state.message = notification.message
      state.details = notification.details
      state.visible = true
    },
    setErrors (state, notification) {
      state.type = notification.type
      state.message = notification.message
      state.details = null
      state.errors = notification.errors
      state.visible = true
    },
    setVisible (state, value) {
      state.visible = value
    }
  }
}