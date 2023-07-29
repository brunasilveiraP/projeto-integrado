import actionWrapper from '@/lib/helpers/action-wrapper'
import Http from '@/lib/http'

export default {
    name: "ModuleUsuario",
    namespaced: true,

    state: {
        listItems: [],
        simNaoEnum: [
            { value: 1, text: "Sim" },
            { value: 2, text: "Não" }
        ],
        calendario: {
            dias: [],
        },
        kwp: {
        },
    },
    
    mutations: {
        SET_LIST_ITEMS(state, payload) {
            state.listItems = payload
        },
        SET_CALENDARIO(state, payload) {
            state.calendario.dias = payload.dias;
        },
        SET_KWP(state, payload) {
            state.kwp = payload.kwp;
        },
    },
    actions: {

        getAll: actionWrapper({
            async action (context, payload) {
                let qrSearch = payload.search != null ? `&SearchText=${payload.search}` : "";
                let response = await Http.get(`/api/services/app/Parceiro/GetAll?SkipCount=${(payload.pageNumber)*payload.itemsPerPage}&MaxResultCount=${payload.itemsPerPage}${qrSearch}`);
                context.state.list = response.data;
                context.commit('SET_LIST_ITEMS', response.data);
            },
            onError(e){
                return e.response.data.error.message;
            }
        }),

        create: actionWrapper({
            async action (context, payload) {
                return await Http.post("/api/services/app/Parceiro/Create", payload);
            },
            onError(e){
                return e.response.data.error.message;
            }
        }),

        delete: actionWrapper({
            async action(context, payload) {
                return await Http.delete(`/api/services/app/Parceiro/Delete?Id=${payload}`)
            },
            async onError(e) {
                return e.response.data.error.message;
            }
        }),

        getById: actionWrapper({
            async action(context, id) {
                return await Http.get(`/api/services/app/Parceiro/Get?Id=${id}`);
            },
            async onError(e) {
                return e.response.data.error.message;
            }
        }),

        getByIdParceiro: actionWrapper({
            async action(context, id) {
                let response = await Http.get(`/api/services/app/Parceiro/GetById?Id=${id}`)
                return response.data.result;
            },
            async onError(e) {
                return e.response.data.error.message;
            }
        }),

        update: actionWrapper({
            async action (context, payload) {
                let response = await Http.put("/api/services/app/Parceiro/Update", payload);
                return response;
            },
            async onError(e) {
                return e.response.data.error.message;
            }
        }),

        getAllParceirosAtivos: actionWrapper({
            async action (_, payload) {
                let response =  await Http.get(`/api/services/app/Parceiro/GetAllParceirosAtivos`)
                return response.data.result;
            },
            async onError(e){
                return e.response.data.error.message;
            }
        }),

        gerarCalendario: actionWrapper({
            action(context, payload) {
                let dias = [];
                for (let index = 1; index <= 31; index++) {
                    dias.push({ value: index, text: index });
                }
                context.commit("SET_CALENDARIO", { dias: dias });
            },
        }),

        gerarKwp: actionWrapper({
            action(context, payload) {
                let kwp = [];
                for (let index = 1; index <= 200; index++) {
                    kwp.push({ value: index, text: index });
                }
                context.commit("SET_KWP", { kwp: kwp });
            },
        }),

        existsCnpj: actionWrapper({
            async action (_, payload) {
                let response =  await Http.get(`/api/services/app/Parceiro/GetExistsCnpj?cnpj=${payload}`)
                return response.data.result;
            },
            async onError(e){
                return e.response.data.error.message;
            }
        }),


    }

}

    

