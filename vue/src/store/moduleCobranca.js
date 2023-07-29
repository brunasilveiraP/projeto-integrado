import actionWrapper from '@/lib/helpers/action-wrapper'
import Http from '@/lib/http'

export default {
    name: "ModuleFase",
    namespaced: true,

    state: {
        listItems: [],
        statusPagamentoEnum: [
            { value: 1, text: "Em aberto" },
            { value: 2, text: "Pago" },
            { value: 4, text: "Não Faturado" },
        ],
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
                let response = await Http.get(`/api/services/app/Cobranca/GetAll?SkipCount=${(payload.pageNumber)*payload.itemsPerPage}&MaxResultCount=${payload.itemsPerPage}${qrSearch}`);
                context.state.list = response.data;
                context.commit('SET_LIST_ITEMS', response.data);
            },
            onError(e){
                return e.response;
            }
        }),

        create: actionWrapper({
            async action (context, payload) {
                return await Http.post("/api/services/app/Cobranca/Create", payload);
            },
            onError(e){
                return e.response;
            }
        }),

        delete: actionWrapper({
            async action(context, payload) {
                return await Http.delete(`/api/services/app/Cobranca/Delete?Id=${payload}`)
            },
            async onError(e) {
                return e.response;
            }
        }),

        getById: actionWrapper({
            async action(context, id) {
                return await Http.get(`/api/services/app/Cobranca/Get?Id=${id}`);
            },
            async onError(e) {
                return e.response;
            }
        }),

        update: actionWrapper({
            async action (context, payload) {
                let response = await Http.put("/api/services/app/Cobranca/Update", payload);
                return response;
            },
            async onError(e) {
                return e.response;
            }
        }),

        confirmarPagamento: actionWrapper({
            async action(context, id) {
                return await Http.post(`/api/services/app/Cobranca/ConfirmarPagamento?CobrancaId=${id}`);
            },
            async onError(e) {
                return e.response;
            }
        }),

        getAllCobrancaByFilter: actionWrapper({
            async action (_, payload) {
                return  await Http.post("/api/services/app/Cobranca/GetAllCobrancaByFilter", payload);
            },
            async onError(e){
                return e.response;
            }
        }),

    }

}

    

