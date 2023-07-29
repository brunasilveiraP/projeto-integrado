<template>
    <div>
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

        <v-form
            ref="form"
            lazy-validation
            class="d-flex flex-row"
            style="flex-wrap: wrap"
        >
            <div class="d-flex flex-row mt-8" style="flex-wrap: wrap; width: 100%">

                <div class="d-flex mb-2" style="width: 100%">

                    <v-select
                        item-value="value"
                        item-text="text"
                        :disabled="true"
                        :items="statusPagamentoEnum"
                        v-model="inputData.statusPagamento"
                        :rules="[rules.required]"
                        label="Status Pagamento"
                        class="mr-4"
                        style="width: 30%"
                        :menu-props="{ bottom: true, offsetY: true }"
                        clearable
                    ></v-select>

                    <calendario
                        label="Data Geração Cobrança"
                        :ruleIndexProps="2"
                        class="mr-4"
                        :disabled="true"
                        :dataProps="inputData.dataGeracao"
                        :required="false"
                        @calendarioDate="inputData.dataGeracao=$event"
                        style="width: 40%"
                    ></calendario>

                    <Money
                        v-model="inputData.valorTotal"
                        :label="$L('Valor Total')"
                        :properties="{
                              prefix: 'R$',
                              readonly: false,
                              disabled: true,
                              outlined: false,
                              dense: false,
                              clearable: true,
                            }"
                        :options="{
                              length: 12,
                              precision: 2,
                              empty: 0,
                            }"
                        class="mr-2 mt-md-4"
                        style="width: 40%"
                    />

                    <calendario
                        label="Data Pagamento"
                        :ruleIndexProps="2"
                        :dataProps="inputData.dataPagamento"
                        :disabled="true"
                        :required="false"
                        style="width: 40%"
                        @calendarioDate="inputData.dataPagamento=$event"
                    ></calendario>

                </div>

                <div class="d-flex mb-2" style="width: 100%">
                    <v-autocomplete
                        id="tabelas"
                        clearable
                        chips
                        item-value="id"
                        item-text="descricao"
                        class="mr-6 required"
                        hide-no-data
                        v-model="inputData.projetosId"
                        label="Selecione projetos..."
                        :items="projetosContempladosValues"
                        multiple
                        deletable-chips
                        :disabled="inputData.statusPagamento == 2"
                    ></v-autocomplete>

                </div>

            </div>
        </v-form>
    </div>
</template>

<script>
import validationRules from "@/mixins/validationRules.js";
import { mapActions,createNamespacedHelpers } from "vuex";
import {
    filtrarCaracterNumerico,
    filtrarCaracterString,
    filtrarCaracterAlfanumericos,
} from "@/lib/helpers/validarCaracteresPermitidos.js";
import Calendario from "@/components/shared/calendario.vue";
import Money from "@/components/shared/Money.vue";

const moduleCobranca = createNamespacedHelpers("ModuleCobranca");
const moduleProjeto = createNamespacedHelpers("ModuleProjeto");

export default {
    props:{
        tenantId: Number
    },
    data: () => ({
        inputData:{
            id: 0,
            tenantId: null,
            statusPagamento: null,
            dataGeracao: null,
            dataPagamento: null,
            valorTotal: null,
            projetosId: []
        },
        visualizacao: false,
        projetosContempladosValues: [],
        alertType: null,
        alertText: '',
        alertShow: false,
    }),

    components:{
        Money,
        Calendario
    },

    computed: {
        statusPagamentoEnum() {
            return this.$store.state.ModuleCobranca.statusPagamentoEnum;
        },

    },

    watch: {
        tenantId(newValue){
            //this.getProjetosContemplados(newValue);
        },
        "inputData.statusPagamento"(newValue){
            if(newValue === 1 || newValue === 2) {this.visualizacao = true};
            this.getProjetosContemplados(this.tenantId)
        },
        "inputData.projetosId"(newValue){
            if(newValue){
                let valorTotal = 0;
                for (let i = 0; i < this.projetosContempladosValues.length; i++) {
                    newValue.forEach((e) => {
                        if (e === this.projetosContempladosValues[i].id) {
                            valorTotal += this.projetosContempladosValues[i].valor;
                        }
                    });
                }
                if(valorTotal !== 0){
                    this.inputData.valorTotal = valorTotal;
                }
            }
        },
    },

    methods: {
        filtrarCaracterNumerico,
        filtrarCaracterString,
        filtrarCaracterAlfanumericos,
        ...moduleCobranca.mapActions(["getAllConcessionarias",]),
        ...moduleProjeto.mapActions(["getAllProjetosParaCobrancaByParceiro"]),
        ...mapActions(["updateTopBarItems"]),

        async getProjetosContemplados(id){
            this.projetosContempladosValues = await this.getAllProjetosParaCobrancaByParceiro({id: id, visualizacao: this.visualizacao});
        },

    },
    mounted() {
    },

    mixins: [
        validationRules,
    ],
};
</script>

<style scoped>
.v-subheader {
    padding: 0px !important;
    color: #153b4c !important;
}

hr {
    opacity: 0.1;
}
</style>