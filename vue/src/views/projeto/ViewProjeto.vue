<template>
    <div class="flex-grow-1">
        <h2 class="bbl-orange--text">Projetos</h2>

        <v-alert
            dismissible
            :type="alertType"
            width="40%"
            style="position: absolute; top: 10%; right: 1%"
            v-show="alertShow"
            transition="scroll-x-reverse-transition"
        >
            {{ alertText }}
        </v-alert>
    
        <modal-delete
            ref="deleteItemRef"
            @isComfirmDeleteItem="confirmDelete($event)"
        />
        
        <modal-parceiro-select
            v-model="showModal"
        />

        <acoes-listagem
            :adicionar="() => (id = 0, showModal = true)"
            :relatorio="() => (drawer = true)"
            :occultRelatorio="true"
            @search="setSearch"
            labelSearch="Pesquisar por Parceiro, Cliente, Concessionária ou Fase"
        />
        
        <filtro-relatorio
        :value="drawer"
        :closeFiltro="() => (drawer = false)"
        @updateValueFiltro="drawer = $event"
        />

        <div class="flex-grow-1 rounded-lg mb-6 white elevation-1">
            <v-data-table
                :key="key"
                :headers="headers"
                :items="rows"
                :options.sync="options"
                :page="page"
                :server-items-length="totalItems"
                noDataText="Nenhum resultado encontrado"
                :loading="tableLoading"
                loading-text="Aguarde, carregando dados..."
                class="flex-grow-1"
                :footer-props="{
                itemsPerPageOptions: [5, 10, 15],
                itemsPerPageText: 'Linhas por Página',
                }"
                :header-props="{sortIcon: null}"
            >
                <template v-slot:[`item.statusPagamento`]="{ item }">
                    {{ item.statusPagamento == 2 ? 'Pago' : item.statusPagamento == 1 ? 'Em Aberto' : 'Não Faturado'}}
                </template>

                <template v-slot:[`item.valor`]="{ item }">
                    {{ formatarMoedaMask(item.valor) }}
                </template>
               
                <!-- Icone Edição -->
                <template v-slot:[`item.edit`]="{ item }">
                    <div class="d-flex flex-nowrap">
                        <v-btn class="bbl-orange--text hover-click" icon @click="setEdition(item.id)">
                            <v-icon>mdi-pencil</v-icon>
                        </v-btn>

                        <v-btn class="bbl-blue--text hover-click" icon @click="openDeleteModal(item.id)" :disabled="isTenant">
                            <v-icon> mdi-delete </v-icon>
                        </v-btn>
                    </div>
                </template>
                
            </v-data-table>
        </div>
    </div>
</template>

<script>

import { mapActions, createNamespacedHelpers } from "vuex"; 
import Money from "@/components/shared/Money";
import AcoesListagem from '@/components/menu/acoesListagem'
import SearchFilters from '@/mixins/SearchFilters'
import ModalParceiroSelect from "@/views/projeto/modal/modalParceiroSelect.vue";
import DataTable from '@/mixins/dataTable'
import ModalDelete from "@/components/shared/modalDelete";
import {formatarMoedaMask} from "@/lib/masks";
import FiltroRelatorio from "@/components/shared/filtroRelatorioProjeto.vue";
import ViewRelatorioProjeto from "@/views/projeto/ViewRelatorioProjeto.vue";

const moduleProjeto = createNamespacedHelpers("ModuleProjeto");

export default {
    name: 'ViewProjeto',

    components: {
        AcoesListagem,
        Money,
        ModalDelete,
        ModalParceiroSelect,
        FiltroRelatorio,
        ViewRelatorioProjeto
    },

    data: () => ({
        headers: [
            { text: "Código", value: "id" },
            { text: "Parceiro", value: "parceiroNome" },
            { text: "Cliente", value: "cliente.nome" },
            { text: "Concessionária", value: "concessionariaNome" },
            { text: "Valor", value: "valor" },
            { text: "Fase", value: "faseNome" },
            { text: "Status Pagto", value: "statusPagamento" },
            { text: "", value: "edit" },
        ],
        id: null,
        rows: [],
        showModal: false,
        modalEditItem: null,
        alertType: null,
        alertText: '',
        alertShow: false,
        key: 1,
        isTenant:false,
        drawer: false,
    }),
    
    methods: {
        formatarMoedaMask,
        ...moduleProjeto.mapActions(["getAll","getAllListing", "create", "delete", "getById"]),

        buildRows(data) {
            this.rows = data;
        },

        setEdition(id) {
            this.id = id;
            this.$router.push({
                name: "ViewManutencaoProjetoEditar",
                params: { id: id },
            });
        },

        async openDeleteModal(itemId) {
            this.$refs.deleteItemRef.open(itemId);
        },
        async confirmDelete(itemId) {
            const isDeleted = await this.delete(itemId);
            if (isDeleted.status === 200) {
                this.key--;
                this.mensagemSucesso("Registro excluído com sucesso!")
            }else{
                this.mensagemErro("Ocorreu um erro ao realizar esta ação. Contate o administrador do sistema.");
            }
        },
        
        mensagemSucesso(mensagem){
            this.alertShow = true;
            this.alertType = "success";
            this.alertText = mensagem;
            setTimeout(() => {
                this.alertShow = false;
            }, 5000);
        },

        mensagemErro(mensagem){
            this.alertShow = true;
            this.alertType = "error";
            this.alertText = mensagem;
            setTimeout(() => {
                this.alertShow = false;
            }, 5000);
        },
        
    },

    mounted() {
        if(this.$store.state.Session.tenant != null){
            this.isTenant = true;
        }
    },
    
    watch:{
    },


    mixins: [
        SearchFilters,        
        DataTable({ store: "ModuleProjeto" }),
    ]
}

</script>

<style scoped>

</style>