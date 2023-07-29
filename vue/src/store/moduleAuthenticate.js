import Util from '@/lib/util';
import Http from '@/lib/http'
import appconst from '@/lib/appconst'
import actionWrapper from '@/lib/helpers/action-wrapper'

export default {
    name: 'ModuleAuthenticate',
    namespaced: true,
    state: {
        isActive:false,
        checkedToChangePassword:false,
        error: {}
    },
    mutations: {
        setError (state, error) {
            state.error = error || {}
        },
    },
    actions: {
        authenticate: actionWrapper({
            async action (_, payload) {
                let response = await Http.post("/api/TokenAuth/Authenticate", payload);
                let tokenExpireDate = payload.rememberMe ? (new Date(new Date().getTime() + 1000 * response.data.result.expireInSeconds)) : undefined;
                Util.abp.auth.setToken(response.data.result.accessToken, tokenExpireDate);
                Util.abp.utils.setCookieValue(appconst.authorization.encrptedAuthTokenName, response.data.result.encryptedAccessToken, tokenExpireDate, Util.abp.appPath);
                _.state.checkedToChangePassword = response.data.result.checkedToChangePassword
                return response;
            },
            async onError(e){
                return e.response;
            }
        }),  

        internalPasswordRecovery: actionWrapper({
          async action (_, payload) {
            return await Http.post('/api/services/app/Identidade/InternalPasswordRecovery',payload);          
          },
          async onError(e){
            return e.response;
          }
        }),

        externalPasswordRecovery: actionWrapper({
          async action (_, payload) {
            return await Http.post(`/api/services/app/Identidade/ExternalPasswordRecovery?email=${payload}`);          
          },
          async onError(e){
            return e.response;
          }
        }),

        alterarSenha: actionWrapper({
          async action (_, payload) {

            return await Http.post("/api/services/app/Identidade/AlterarSenha", payload.data);          
          },
          async onError(e){

            return e.response
          }
        }), 
    }
}
