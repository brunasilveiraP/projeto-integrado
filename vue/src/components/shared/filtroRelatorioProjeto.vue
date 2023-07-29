<template>
    <div>
        <view-relatorio-projeto v-model="dialog" :dataResult="conteudo" :cabecalho="inputData" @updateValueDialog="dialog = $event"></view-relatorio-projeto>

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
                <diV class="input-box required ml-4 mr-4">
                    <CalendarioPeriodo
                        v-model="date"
                        @changeValue="changeValue($event)"
                        :isIcon="true"
                        compareRange
                        label="Período de Vigência"
                    >
                    </CalendarioPeriodo>

                </diV>
                <div class="input-box required  ml-4 mr-4">

                </div>
            </div>

            <div class="d-flex mt-4 ml-4">

                <v-autocomplete
                    :items="fases"
                    item-value="id"
                    item-text="nome"
                    v-model="inputData.fases"
                    label="Fase"
                    class="mr-4 required"
                    style="width: 40%"
                    :menu-props="{ bottom: true, offsetY: true }"
                    clearable
                    multiple
                    chips
                    deletable-chips
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

const moduleFase = createNamespacedHelpers("ModuleFase");
const moduleParceiro = createNamespacedHelpers("ModuleParceiro");
const moduleProjeto = createNamespacedHelpers("ModuleProjeto");
export default {
    name: "filtroRelatorioProjeto",
    props:{
        value: {type: Boolean, default: false},
    },
    components:{
        Calendario,
        ViewRelatorioProjeto,
        CalendarioPeriodo
    },
    data: () => ({
        inputData:{
            parceiros: [],
            dataInicio: null,
            dataFim: null,
            fases: [],
        },
        parceiros: [],
        fases: [],
        conteudo:[],
        dialog: false,
        openDrawer: false,
        date: [],
    }),
    methods:{
        ...moduleParceiro.mapActions(["getAllParceirosAtivos"]),
        ...moduleFase.mapActions(["getAllFases",]),
        ...moduleProjeto.mapActions(["getAllProjetoByFilter"]),

        async getParceiros(){
            this.parceiros = await this.getAllParceirosAtivos();
        },
        async getFases(){
            this.fases = await this.getAllFases();
        },
        async getProjetosByFilterRelatorio(){
            let response = await this.getAllProjetoByFilter(this.inputData);
            if(response.status === 200){
                this.conteudo = response.data.result;
            }else{
                this.mensagemErro("Ocorreu um erro ao realizar esta ação. Contate o administrador do sistema.");
            }
        },
        async getConteudoRelatorio(){
            await this.getProjetosByFilterRelatorio();
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
    },
    mounted() {
        this.getParceiros();
        this.getFases();
    },
    watch:{
        value(newValue){
            this.openDrawer = newValue;
        },
        openDrawer(newValue){
            if(!newValue){
                this.date = null;
                this.inputData.dataInicio = null;
                this.inputData.dataFim = null;
                this.inputData.parceiros = [];
                this.inputData.fases = [];
            }
            this.$emit("updateValueFiltro", newValue);
        }
    },
    computed:{
        disableFilterAction(){
            return !(this.inputData.dataInicio && this.inputData.dataFim && this.inputData.fases.length >0);
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