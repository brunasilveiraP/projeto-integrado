<template>
    <v-dialog
        persistent
        v-model="dialog"
        transition="dialog-bottom-transition"
        class="atendimento-container"
        fullscreen>
        <template>

            <v-card >
                
                <v-card-text id="print-card-text">
                    <v-row>
                        <v-col cols="2">
                            <v-col class="textBold mt-5">
                                <h2>BBL GESTAO</h2>
                            </v-col>
                        </v-col>
                        <v-col cols="10" class="mt-6">
                            <v-row>
                                Relatório de Cobranças
                            </v-row>
                            <v-row class="my-2 mb-2 mt-6" >
                                Periodo: {{cabecalho.dataInicio}} a {{cabecalho.dataFim}}
                            </v-row>
                            <v-row class="mb-1">
                                <v-divider></v-divider>
                            </v-row>
                        </v-col>
                    </v-row>

                    <v-row class="justify-center">
                        <v-col cols="3" class="textBold">Parceiro</v-col>
                        <v-col cols="3" class="textBold">Data Geração</v-col>
                        <v-col cols="2" class="textBold">Data Pagamento</v-col>
                        <v-col cols="2" class="textBold">Status</v-col>
                        <v-col cols="2" class="textBold">Valor</v-col>
                    </v-row>
                    
                    <v-row class="mb-0">
                        <v-col cols="12"><v-divider></v-divider></v-col>
                    </v-row>
                    
                    <v-row v-if="itens" v-for="item in itens" :key="item.valor" class="justify-center">
                        <v-col cols="3" class="borderBottom" >{{item.parceiroNome}}</v-col>
                        <v-col cols="3" class="borderBottom">{{item.dataGeracao | date}}</v-col>
                        <v-col cols="2" class="borderBottom">{{item.dataPagamento | date}}</v-col>
                        <v-col cols="2" class="borderBottom"> {{mapStatus[item.statusPagamento]}}</v-col>
                        <v-col cols="2" class="borderBottom">{{ formatarMoedaMask(item.valorTotal) }}</v-col>
                    </v-row>
                    
                    <v-row  v-else class="justify-center">Não foram encontrados resultados para este filtro</v-row>
                    
                    <v-row class="justify-center mb-1">
                        <v-col cols="3"></v-col>
                        <v-col cols="3"></v-col>
                        <v-col cols="2"></v-col>
                        <v-col cols="2">Total</v-col>
                        <v-col cols="2">{{ formatarMoedaMask(valorTotal) }}</v-col>
                    </v-row>
                </v-card-text>

                <v-footer class="justify-end my-1" inset app>
                    <v-row class="justify-end">
                        <v-col cols="10"></v-col>
                        <v-col cols="2">
                            <v-btn text fab @click="imprimirRelatorio" class="hover-click ml-6 primary">
                                <v-icon>mdi-printer</v-icon>
                            </v-btn>

                            <v-btn text fab @click="exit" class="hover-click bbl-blue--text">
                                <v-icon>mdi-close</v-icon>
                            </v-btn>
                        </v-col>
                    </v-row>
                </v-footer>
            </v-card>
        </template>
    </v-dialog>
</template>

<script>

import { mapActions, createNamespacedHelpers } from "vuex";
import SearchFilters from '@/mixins/SearchFilters'
import DataTable from '@/mixins/dataTable'
import {formatarMoedaMask} from "@/lib/masks";
import printJS from "print-js";

const moduleProjeto = createNamespacedHelpers("ModuleProjeto");

export default {
    name: 'ViewRelatorioCobranca',

    components: {
    },

    props:{
        value: {
            type: Boolean,
            required: false,
            default: false,
        },
        dataResult: [],

        cabecalho: {
            type: Object,
            required: false,
            default: null
        }
    },

    data: () => ({
        itens: [
        ],
        isTenant:false,
        dialog: false,
        valorTotal: null,
        mapStatus : {
            1: 'Em Aberto',
            2: 'Pago',
            4: 'Não Faturado'
        }
    }),

    methods: {
        formatarMoedaMask,

        exit(){
            this.dialog = false;
            this.$emit("updateValueDialog", this.dialog);
        },

        imprimirRelatorio() {

            let stylesHtml =
                '.row {\n' +
                '    font-family: "Open Sans", sans-serif;\n' +
                '}';
            for (const node of [...document.querySelectorAll('head > style')]) {
                stylesHtml += node.outerHTML;
            }

            printJS({
                printable: 'print-card-text',
                type: 'html',
                scanStyles: false,
                css: ['https://fonts.googleapis.com/css2?family=Open+Sans:wght@300;400;500;600;700;800&display=swap, https://cdn.jsdelivr.net/npm/@mdi/font@6.x/css/materialdesignicons.min.css'],
                style: stylesHtml
            })
        },
    },

    mounted() {

    },

    watch: {
        value(newValue) {
            this.dialog = newValue;
        },
        dataResult(newValue){
            this.itens = newValue;
            let valorTotal = 0;
            this.itens.forEach( (item) => {
                valorTotal += item.valorTotal;
            });
            this.valorTotal = valorTotal;
        },
    },


    mixins: [
        SearchFilters,
        DataTable({ store: "ModuleProjeto" }),
    ]
}

</script>

<style scoped>

.atendimento-container{
    width: 100vw;
    z-index: 1000;
}

.borderBottom{
    border-style: none none solid none;
    border-width: 0.5px;
    border-color: #eeeeee;
}


@media print {

}



</style>