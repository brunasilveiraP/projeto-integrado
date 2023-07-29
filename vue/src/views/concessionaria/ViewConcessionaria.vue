<template>
    <div class="flex-grow-1">
        <h2 class="bbl-orange--text">Concessionárias</h2>

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
    
        <modal-concessionaria 
            v-model="showModal"
            :edit-id="this.id"
            @atualizaTabela="key++"
            @mensagemSucesso="mensagemSucesso"
        />

        <modal-delete
            ref="deleteItemRef"
            @isComfirmDeleteItem="confirmDelete($event)"
        />

        <acoes-listagem
            :adicionar="() => (id = 0, showModal = true)"
            :occultRelatorio="true"
            @search="setSearch"
            labelSearch="Pesquisar por Código ou Nome"
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
               
                <!-- Icone Edição -->
                <template v-slot:[`item.edit`]="{ item }">
                    <div class="d-flex flex-nowrap">
                        <v-btn class="bbl-orange--text hover-click" icon @click="setEdition(item.id)">
                            <v-icon>mdi-pencil</v-icon>
                        </v-btn>

                        <v-btn class="bbl-blue--text hover-click" icon @click="openDeleteModal(item.id)">
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
import ModalConcessionaria from "@/views/concessionaria/modal/modalConcessionaria.vue";
import DataTable from '@/mixins/dataTable'
import ModalDelete from "@/components/shared/modalDelete";

const moduleConcessionaria = createNamespacedHelpers("ModuleConcessionaria");

export default {
    name: 'ViewConcessionaria',

    components: {
        AcoesListagem,
        ModalConcessionaria,
        Money,
        ModalDelete,
    },

    data: () => ({
        headers: [
            { text: "Código", value: "id" },
            { text: "Nome", value: "nome" },
            { text: "E-mail", value: "email" },
            { text: "Telefone", value: "telefone" },
            { text: "", value: "edit" },
        ],
        id: null,
        rows: [],
        showModal: false,
        modalEditItem: null,
        dialogDeleteConfirm: false,
        alertType: null,
        alertText: '',
        alertShow: false,
        key: 1,
    }),

    methods: {
        ...moduleConcessionaria.mapActions(["getAll", "create", "delete", "getById"]),

        buildRows(data) {
            this.rows = data;
        },

        setEdition(id) {
            this.id = id;
            this.showModal = true;
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
        }
    },

    mixins: [
        SearchFilters,        
        DataTable({ store: "ModuleConcessionaria" }),
    ]
}

</script>

<style scoped>

</style>