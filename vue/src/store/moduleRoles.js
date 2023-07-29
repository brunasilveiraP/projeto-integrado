import Http from '@/lib/http'
import actionWrapper from '@/lib/helpers/action-wrapper'

export default {

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

        getAllRoles: actionWrapper({
            async action (context, payload) {
                try{
                    let response = await Http.get(`/api/services/app/Role/GetAll`);
                    var roles = []
                    response.data.result.items.forEach(element => {
                        roles.push({
                            value : element.id,
                            text : element.name
                        })}
                    );
                    context.commit('SET_LIST_ITEMS', response.data.result.items)
                    return roles;
                }catch(e){
                    return e;
                }
            }
        }),
    }
}