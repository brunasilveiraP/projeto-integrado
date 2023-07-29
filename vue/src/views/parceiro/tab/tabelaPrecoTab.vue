<template>
    <div>
        <v-form
            ref="form"
            lazy-validation
            class="d-flex flex-row"
            style="flex-wrap: wrap"
        >
            <div class="d-flex flex-row mt-4" style="flex-wrap: wrap; width: 100%">

                <div class="d-flex" style="width: 100%">
                    <v-subheader class="d-flex align-center black--text textBold">Informações Financeiras</v-subheader>
                </div>
                <div class="d-flex" style="width: 100%">
                    <v-select
                        v-model="inputData.diasPagamento"
                        class="input-box required mr-4"
                        label="Dia(s) de Pagamento"
                        multiple
                        clearable
                        :items="getCalendario.dias"
                        :menu-props="{ bottom: true, offsetY: true }"
                    />
                       
                    <v-select
                        item-value="value"
                        item-text="text"
                        :items="pagaTrtEnum"
                        v-model="inputData.pagaTrt"
                        :rules="[rules.required]"
                        label="Parceiro Paga TRT ?"
                        class="mr-4 required"
                        style="width: 30%"
                        :menu-props="{ bottom: true, offsetY: true }"
                        clearable
                    ></v-select>
                </div>
            </div>

            <div class="d-flex mt-4" style="width: 100%">
                <v-subheader class="d-flex align-center blue-grey--text textBold">Os valores dos projetos serão calculados a partir desta referência.</v-subheader>
            </div>

            <div class="d-flex mt-1" style="width:100%">

                <v-autocomplete
                    id="tabelas"
                    clearable
                    chips
                    item-value="id"
                    item-text="descricao"
                    class="mr-6 required"
                    hide-no-data
                    v-model="inputData.tabelasPreco"
                    label="Selecione as tabelas de preço..."
                    :items="tabelasPrecoValues"
                    multiple
                    deletable-chips
                ></v-autocomplete>
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
    filtrarCaracterNumericoComVirgula,
} from "@/lib/helpers/validarCaracteresPermitidos.js";

const moduleTabelaPreco = createNamespacedHelpers("ModuleTabelaPreco");

export default {
    components:{
    },
    data: () => ({
        fieldLoading: false,
        tabelasPrecoValues: [],
        inputData: {
            id: 0,
            tabelasPreco: [],
            diasPagamento: [],
            pagaTrt: null,
        },
    }),

    methods: {
        filtrarCaracterNumerico,
        filtrarCaracterString,
        filtrarCaracterAlfanumericos,
        filtrarCaracterNumericoComVirgula,
        ...moduleTabelaPreco.mapActions(["getAllTabelasPrecoForEnum"]),
        ...mapActions("ModuleParceiro", ["gerarCalendario"]),

        async getTabelasPreco(){
            this.tabelasPrecoValues = await this.getAllTabelasPrecoForEnum();
        },

    },
    mounted() {
        this.getTabelasPreco();
        this.gerarCalendario();
    },

    computed: {
        pagaTrtEnum() {
            return this.$store.state.ModuleParceiro.simNaoEnum;
        },
        getCalendario() {
            return this.$store.state.ModuleParceiro.calendario;
        },
    },
    mixins: [
        validationRules,
    ],
}
</script>

<style scoped>
.v-subheader {
    padding: 0px !important;
}
</style>