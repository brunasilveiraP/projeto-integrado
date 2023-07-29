import actionWrapper from '@/lib/helpers/action-wrapper'
import Http from '@/lib/http'

export default {
    name: "ModuleTipoAnexo",
    namespaced: true,

    state: {
        listItems: [],
    },
    
    mutations: {
        SET_LIST_ITEMS(state, payload) {
            state.listItems = payload
        },
    },
    actions: {
        getAll: actionWrapper({
            async action (context, payload) {
                let qrSearch = payload.search != null ? `&SearchText=${payload.search}` : "";
                let response = await Http.get(`/api/services/app/TipoAnexo/GetAll?SkipCount=${(payload.pageNumber)*payload.itemsPerPage}&MaxResultCount=${payload.itemsPerPage}${qrSearch}`);
                context.commit('SET_LIST_ITEMS', response.data);
            },
            onError(e){
                return e.response.data.err.message;
            }
        }),

        getAllTiposAnexo: actionWrapper({
            async action (context, payload) {
                let response = await Http.get(`/api/services/app/TipoAnexo/GetAll`);
                return response.data.result.items;
            },
            onError(e){
                return e.response.data.err.message;
            }
        }),

        create: actionWrapper({
            async action (context, payload) {
                return await Http.post("/api/services/app/TipoAnexo/Create", payload);
            },
            onError(e){
                return e.response.data.err.message;
            }
        }),

        delete: actionWrapper({
            async action(context, payload) {
                return await Http.delete(`/api/services/app/TipoAnexo/Delete?Id=${payload}`)
            },
            async onError(e) {
                return e.response;
            }
        }),

        getById: actionWrapper({
            async action(context, id) {
                return await Http.get(`/api/services/app/TipoAnexo/Get?Id=${id}`);
            },
            async onError(e) {
                return e.response;
            }
        }),

        update: actionWrapper({
            async action (context, payload) {
                let response = await Http.put("/api/services/app/TipoAnexo/Update", payload);
                return response;
            },
            async onError(e) {
                return e.response;
            }
        }),

    }

}

    

