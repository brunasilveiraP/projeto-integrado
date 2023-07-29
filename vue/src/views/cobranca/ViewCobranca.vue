<template>
    <div class="flex-grow-1">
        <h2 class="bbl-orange--text py-3">Cobranças</h2>

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

        <modal-parceiro-select-cobranca
            v-model="showModal"
        />

        <modal-delete
            ref="deleteItemRef"
            @isComfirmDeleteItem="confirmDelete($event)"
        />
        
        <filtro-relatorio-cobranca
            :value="drawer"
            :closeFiltro="() => (drawer = false)"
            @updateValueFiltro="drawer = $event"
        />
        
        <acoes-listagem
            :adicionar="() => (id = 0, showModal = true)"
            :relatorio="() => (drawer = true)"
            @search="setSearch"
            :occultRelatorio="true"
            labelSearch="Pesquisar por Parceiro"
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
                itemsPerPageOptions: [ 10, 15],
                itemsPerPageText: 'Linhas por Página',
                }"
                :header-props="{sortIcon: null}"
            >

                <template v-slot:[`item.dataGeracao`]="{ item }">
                    {{ item.dataGeracao != null ? formatarTime(item.dataGeracao) : " - "}}
                </template>

                <template v-slot:[`item.valorTotal`]="{ item }">
                    {{ item.valorTotal | currencyFormatReal   }}
                </template>
                
                <template v-slot:[`item.statusPagamento`]="{ item }">
                    {{ item.statusPagamento != null ?
                    statusPagamentoEnum.filter(itemF => itemF.value === item.statusPagamento)[0].text:
                        "Não faturado"
                    }}
                </template>

                <template v-slot:[`item.dataPagamento`]="{ item }">
                    {{ item.dataPagamento != null ? formatarTime(item.dataPagamento) : " - "}}
                </template>
               
                <!-- Icone Edição -->
                <template v-slot:[`item.edit`]="{ item }">
                    <div class="d-flex flex-nowrap">
                        <v-btn class="bbl-orange--text hover-click" icon @click="setEdition(item.id)">
                            <v-icon>mdi-pencil</v-icon>
                        </v-btn>

                        <v-btn class="bbl-blue--text hover-click" icon @click="openDeleteModal(item)">
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
import DataTable from '@/mixins/dataTable'
import ModalDelete from "@/components/shared/modalDelete";
import ModalParceiroSelectCobranca from "@/views/cobranca/modal/modalParceiroSelectCobranca.vue";
import {formatarTime} from "../../lib/timeStamp";
import FiltroRelatorioCobranca from "@/components/shared/filtroRelatorioCobranca.vue";

const moduleCobranca = createNamespacedHelpers("ModuleCobranca");

export default {
    name: 'ViewCobranca',

    components: {
        ModalParceiroSelectCobranca,
        AcoesListagem,
        Money,
        ModalDelete,
        FiltroRelatorioCobranca,
    },

    data: () => ({
        headers: [
            { text: "Código", value: "id" },
            { text: "Parceiro", value: "parceiroNome" },
            { text: "Data Geração", value: "dataGeracao" },
            { text: "Valor", value: "valorTotal" },
            { text: "Status", value: "statusPagamento" },
            { text: "Data Pagamento", value: "dataPagamento" },
            { text: "Projetos Contemplados", value: "quantidadeProjetos" },
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
        drawer: null,
    }),

    methods: {
        formatarTime,
        ...moduleCobranca.mapActions(["getAll", "create", "delete", "getById"]),

        buildRows(data) {
            this.rows = data;
        },

        setEdition(id) {
            this.id = id;
            this.$router.push({
                name: "ViewManutencaoCobrancaEditar",
                params: { id: id },
            });
        },
        async openDeleteModal(item) {
            if(item.statusPagamento !== 2)
                this.$refs.deleteItemRef.open(item.id);
            else
                this.mensagemErro("Já foi identificado pagamento para esta cobrança. Não é permitida a exclusão.");
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

    computed: {
        statusPagamentoEnum() {
            return this.$store.state.ModuleCobranca.statusPagamentoEnum;
        },
    },

    mixins: [
        SearchFilters,        
        DataTable({ store: "ModuleCobranca" }),
    ]
}

</script>

<style scoped>

</style>