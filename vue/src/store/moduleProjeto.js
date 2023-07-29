import actionWrapper from '@/lib/helpers/action-wrapper'
import Http from '@/lib/http'

export default {
    name: "ModuleUsuario",
    namespaced: true,

    state: {
        listItems: [],
        tipoGeracaoEnum: [
            { value: 1, text: "Geração Própria" },
            { value: 2, text: "Auto-Consumo Remoto" },
            { value: 4, text: "Geração Compartilhada" },
            { value: 8, text: "Emuc" }
        ],
        tipoAterramentoEnum: [
            { value: 1, text: "TR" },
            { value: 2, text: "TNS" },
            { value: 4, text: "TNST" },
            { value: 8, text: "TT" }
        ],
        tipoPessoa: [
            { value: 1, text: "Pessoa Física" },
            { value: 2, text: "Pessoa Jurídica" }
        ],
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
                let response = await Http.get(`/api/services/app/Projeto/GetAll?SkipCount=${(payload.pageNumber)*payload.itemsPerPage}&MaxResultCount=${payload.itemsPerPage}${qrSearch}`);
                context.state.list = response.data;
                context.commit('SET_LIST_ITEMS', response.data);
            },
            onError(e){
                return e.response.data.error.message;
            }
        }),

        getAllListing: actionWrapper({
            async action (context, payload) {
                let qrSearch = payload.search != null ? `&SearchText=${payload.search}` : "";
                let response = await Http.get(`/api/services/app/Projeto/GetAllListing?SkipCount=${(payload.pageNumber)*payload.itemsPerPage}&MaxResultCount=${payload.itemsPerPage}${qrSearch}`);
                context.state.list = response.data;
                context.commit('SET_LIST_ITEMS', response.data);
            },
            onError(e){
                return e.response.data.error.message;
            }
        }),

        create: actionWrapper({
            async action (context, payload) {
                return await Http.post("/api/services/app/Projeto/Create", payload);
            },
            onError(e){
                return e.response.data.error.message;
            }
        }),

        delete: actionWrapper({
            async action(context, payload) {
                return await Http.delete(`/api/services/app/Projeto/Delete?Id=${payload}`)
            },
            async onError(e) {
                return e.response.data.error.message;
            }
        }),

        getById: actionWrapper({
            async action(context, id) {
                return await Http.get(`/api/services/app/Projeto/Get?Id=${id}`);
            },
            async onError(e) {
                return e.response.data.error.message;
            }
        }),

        update: actionWrapper({
            async action (context, payload) {
                let response = await Http.put("/api/services/app/Projeto/Update", payload);
                return response;
            },
            async onError(e) {
                return e.response.data.error.message;
            }
        }),

        getAllProjetosParaCobrancaByParceiro: actionWrapper({
            async action (_, payload) {
                let response =  await Http.get(`/api/services/app/Projeto/GetAllProjetosParaCobranca?ParceiroId=${payload.id}&IsVisualizacao=${payload.visualizacao}`)
                return response.data.result;
            },
            async onError(e){
                return e.response;
            }
        }),

        getAllProjetoByFilter: actionWrapper({
            async action (_, payload) {
                return  await Http.post("/api/services/app/Projeto/GetAllProjetoByFilter", payload);
            },
            async onError(e){
                return e.response;
            }
        }),

        

    }

}

    

