<template>
    <div>
        <view-relatorio v-model="dialog" :dataResult="conteudo" :cabecalho="inputData" @updateValueDialog="dialog = $event"></view-relatorio>

        <v-navigation-drawer
            v-model="openDrawer"
            absolute
            bottom
            right
            width="380px"
            height="100%"
            class="elevation-2"
        >
            <v-subheader class="textBold black--text">Filtros</v-subheader>
            <v-divider class="dividerColor"></v-divider>


            <div class="d-flex mt-4 ml-4">

                <v-autocomplete
                    :items="parceiros"
                    item-value="id"
                    item-text="fantasia"
                    v-model="inputData.parceiros"
                    label="Parceiro"
                    class="mr-4"
                    style="width: 40%"
                    :menu-props="{ bottom: true, offsetY: true }"
                    clearable
                    multiple
                    chips
                    deletable-chips
                    hide-no-data
                />
            </div>

            <div class="input-group-container">
                <diV class="input-box ml-4 mr-4">
                    <CalendarioPeriodo
                        v-model="date"
                        @changeValue="changeValue($event)"
                        :isIcon="true"
                        compareRange
                        label="Período de Geração"
                    />

                    <CalendarioPeriodo
                        v-model="datePagamento"
                        @changeValue="changeValuePagamento($event)"
                        :isIcon="true"
                        :required="false"
                        compareRange
                        label="Período de Pagamento"
                    />

                </diV>
                <div class="input-box required  ml-4 mr-4">

                </div>
            </div>

            <div class="d-flex mt-4 ml-4">

                <v-autocomplete
                    :items="listaStatus"
                    v-model="inputData.status"
                    label="Status"
                    class="mr-4"
                    style="width: 40%"
                    :menu-props="{ bottom: true, offsetY: true }"
                    clearable
                    hide-no-data
                />
            </div>


            <v-footer class="justify-end my-1 d-flex" app padless inset>
                <v-col cols="12">
                    <v-row>
                        <v-divider class="dividerColor"></v-divider>
                    </v-row>
                    <v-row class="justify-end" >
                        <v-col cols="4" class="mt-1">
                            <v-btn class="px-5 font-weight-bold hover-click" text @click="()=> (openDrawer = !openDrawer)">CANCELAR</v-btn>
                        </v-col>
                        <v-col class="mt-1 mr-1" cols="4">
                            <v-btn class="bbl-orange pr-5 hover-click px-5 font-weight-bold white--text" @click="getConteudoRelatorio" :disabled="disableFilterAction">FILTRAR</v-btn>
                        </v-col>
                    </v-row>

                </v-col>


            </v-footer>

        </v-navigation-drawer>
    </div>
</template>

<script>

import {createNamespacedHelpers} from "vuex";
import Calendario from "@/components/shared/calendario.vue";
import ViewRelatorioProjeto from "@/views/projeto/ViewRelatorioProjeto.vue";
import CalendarioPeriodo from "@/components/shared/calendarioPeriodo.vue";
import ViewRelatorio from "@/views/cobranca/ViewRelatorioCobranca.vue";

const moduleParceiro = createNamespacedHelpers("ModuleParceiro");
const moduleCobranca = createNamespacedHelpers("ModuleCobranca");
export default {
    name: "filtroRelatorioCobranca",
    props:{
        value: {type: Boolean, default: false},
    },
    components:{
        Calendario,
        ViewRelatorioProjeto,
        CalendarioPeriodo,
        ViewRelatorio,
    },
    data: () => ({
        inputData:{
            parceiros: [],
            dataInicio: null,
            dataFim: null,
            dataPagamentoInicio: null,
            dataPagamentoFim: null,
            status: null,
        },
        parceiros: [],
        status: [],
        conteudo:[],
        dialog: false,
        openDrawer: false,
        date: [],
        datePagamento: [],
        listaStatus: [
            {value: 1, text: "Em Aberto"},
            {value: 2, text: "Pago"},
            {value: 4, text: "Não Faturado"},
        ]
    }),
    methods:{
        ...moduleParceiro.mapActions(["getAllParceirosAtivos"]),
        ...moduleCobranca.mapActions(["getAllCobrancaByFilter"]),

        async getParceiros(){
            this.parceiros = await this.getAllParceirosAtivos();
        },
        async getCobrancasByFilterRelatorio(){
            let response = await this.getAllCobrancaByFilter(this.inputData);
            if(response.status === 200){
                this.conteudo = response.data.result;
            }else{
                this.mensagemErro("Ocorreu um erro ao realizar esta ação. Contate o administrador do sistema.")
            }
        },
        async getConteudoRelatorio(){
            await this.getCobrancasByFilterRelatorio();
            this.dialog = true;
        },
        async changeValue(event){
            if (event.length === 1) {
                this.inputData.dataInicio = event[0];
            }
            if (event.length === 2) {
                this.inputData.dataInicio = event[0];
                this.inputData.dataFim = event[1];
            }
        },
        async changeValuePagamento(event){
            if (event.length === 1) {
                this.inputData.dataPagamentoInicio = event[0];
            }
            if (event.length === 2) {
                this.inputData.dataPagamentoInicio = event[0];
                this.inputData.dataPagamentoFim = event[1];
            }
        },
    },
    mounted() {
        this.getParceiros();
    },
    watch:{
        value(newValue){
            this.openDrawer = newValue;
        },
        openDrawer(newValue){
            if(!newValue){
                this.date = null;
                this.datePagamento = null,
                this.inputData.dataInicio = null;
                this.inputData.dataFim = null;
                this.inputData.dataPagamentoInicio = null;
                this.inputData.dataPagamentoFim = null;
                this.inputData.parceiros = [];
                this.inputData.status = null;
            }
            this.$emit("updateValueFiltro", newValue);
        }
    },
    computed:{
        disableFilterAction(){
            return !(this.inputData.dataInicio && this.inputData.dataFim);
        }
    }
}
</script>

<style scoped>

.v-subheader {
    font-size: 18px;
    color: #4D4D4D;
}
.dividerColor{
    background-color: #d3d3d3;
}

.v-footer {
    background-color: #ffffff;
}

</style>