import actionWrapper from '@/lib/helpers/action-wrapper'
import Http from '@/lib/http'

export default {
    name: "ModuleUsuario",
    namespaced: true,

    state: {
        listItems: [],
    },
    
    mutations: {
        SET_LIST_ITEMS(state, payload) {
            state.listItems = payload
        }
    },
    actions: {

        getAll: actionWrapper({
            async action (context, payload) {
                let qrSearch = payload.search != null ? `&SearchText=${payload.search}` : "";
                let response = await Http.get(`/api/services/app/ProjetoObservacao/GetAll?SkipCount=${(payload.pageNumber)*payload.itemsPerPage}&MaxResultCount=${payload.itemsPerPage}${qrSearch}`);
                context.state.list = response.data;
                context.commit('SET_LIST_ITEMS', response.data);
            },
            onError(e){
                return e.response.data.error.message;
            }
        }),

        getAllObservacoes: actionWrapper({
            async action(context, payload) {
                const response = await Http.get(
                    `/api/services/app/ProjetoObservacao/GetAll`,
                    { params: payload }
                );
                
                return response;
            },
            async onError(e) {
                return e.response;
            },
        }),

        create: actionWrapper({
            async action (context, payload) {
                return await Http.post("/api/services/app/ProjetoObservacao/Create", payload);
            },
            onError(e){
                return e.response.data.error.message;
            }
        }),

        delete: actionWrapper({
            async action(context, payload) {
                return await Http.delete(`/api/services/app/ProjetoObservacao/Delete?Id=${payload}`)
            },
            async onError(e) {
                return e.response.data.error.message;
            }
        }),

        getById: actionWrapper({
            async action(context, id) {
                return await Http.get(`/api/services/app/ProjetoObservacao/Get?Id=${id}`);
            },
            async onError(e) {
                return e.response.data.error.message;
            }
        }),

        update: actionWrapper({
            async action (context, payload) {
                let response = await Http.put("/api/services/app/ProjetoObservacao/Update", payload);
                return response;
            },
            async onError(e) {
                return e.response.data.error.message;
            }
        }),

    }

}

    

