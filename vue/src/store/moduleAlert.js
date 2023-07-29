import actionWrapper from '@/lib/helpers/action-wrapper'
export default {
    namespaced: true,

    state: {
        message: null,
        type: null,
        displayAlert: false,

        displayAlertModal: false,
        typeAlertModal: null,
        messageAlertModal: null,
    },
    mutations: {
        SET_STATE_ALERT(state, payload) {
            state.message = payload.message
            state.type = payload.type
            state.displayAlert = payload.displayAlert
        },
        SET_ALERT_MODAL(state, payload) {
            state.messageAlertModal = payload.message
            state.typeAlertModal = payload.type
        },

        RESET_STATE_ALERT(state, payload) {
            state.message = null
            state.type = null
            state.displayAlert = false
        },

        RESET_ALERT_MODAL(state, payload) {
            state.messageAlertModal = null
            state.typeAlertModal = null
            state.displayAlertModal = false
        }

    },
    actions: {
        criarAlerta: actionWrapper({
            action(context, payload) {
                if(payload.modalAlert) {
                    context.commit('SET_ALERT_MODAL', payload);
                }
                else {
                    context.commit('SET_STATE_ALERT', payload);
                }

                setTimeout(() => {
                    context.commit('RESET_STATE_ALERT', payload);
                    context.commit('RESET_ALERT_MODAL', payload);
                }, 5000);
            }
        }),
    }
}