/* eslint-disable */
import axios from 'axios'
import store from '@/store'
import appconst from './appconst'
import { createNamespacedHelpers } from 'vuex'

const http = axios.create({
  baseURL: appconst.remoteServiceBaseUrl,
  timeout: 84000
})

http.interceptors.request.use(function (config) {
  if (!!window.abp.auth.getToken()) {
    config.headers.common["Authorization"] = "Bearer " + window.abp.auth.getToken();
  }
  config.headers.common[".AspNetCore.Culture"] = window.abp.utils.getCookieValue("Abp.Localization.CultureName");
  config.headers.common["Abp.TenantId"] = window.abp.multiTenancy.getTenantIdCookie();
  return config;
}, function (error) {
    return Promise.reject(error);
})


http.interceptors.response.use((respon) => {

    return respon
}, (error) => {
    if(error.response.status === 400 && error.response.data.error.code === 547){
        store.dispatch(
            'ModuleAlert/criarAlerta',
            {message: 'O registro não pode ser excluído pois possui vínculos.', type: 'error',  displayAlert: true }
        )
    } else if (error.response.status === 401 || error.response.status === 404) {
        window.location.href = "/";
    }
    else if (error.response.status === 403) {
        store.dispatch(
            'ModuleAlert/criarAlerta',
            {message: 'Seu usuário não possui permissão para realizar esta ação.', type: 'error',  displayAlert: true }
        )
    } 
    else {
        store.dispatch({
            type: 'Notification/notify',
            data: {
                type: 'error',
                message: 'Erro inesperado',
                details: 'Ocorreu um erro inesperado, tente novamente!'
            }
        })
    }
    return Promise.reject(error);
});


export default http;