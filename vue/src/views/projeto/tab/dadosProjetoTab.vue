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

                    <v-autocomplete
                        :items="fases"
                        item-value="id"
                        item-text="nome"
                        v-model="fase"
                        label="Fase"
                        :rules="[rules.required]"
                        class="mr-4 required"
                        style="width: 40%"
                        :menu-props="{ bottom: true, offsetY: true }"
                        clearable
                        :disabled="tenantId !== null || disabledFields"
                    />

                    <v-autocomplete
                        :items="concessionarias"
                        item-value="id"
                        item-text="nome"
                        v-model="concessionaria"
                        label="Concessionária"
                        :rules="[rules.required]"
                        class="mr-4 required"
                        style="width: 40%"
                        :menu-props="{ bottom: true, offsetY: true }"
                        clearable
                        :disabled="disabledFields"
                    />
                    
                    <v-text-field
                        v-model="inputData.protocolo"
                        label="Protocolo"
                        persistent-hint
                        hint="Somente números"
                        class="mr-4"
                        style="width: 40%"
                        clearable
                        validate-on-blur
                        :loading="loading"
                        :menu-props="{ bottom: true, offsetY: true }"
                        :disabled="disabledFields"
                    ></v-text-field>
                </div>

                <div class="d-flex mb-2" style="width: 100%">
                    <v-text-field
                        v-model="inputData.fabricanteInversor"
                        :rules="[rules.required]"
                        label="Fabricante Inversor"
                        class="mr-4 required"
                        style="width: 38%"
                        :loading="loading"
                        clearable
                        :disabled="disabledFields"
                    ></v-text-field>

                    <v-text-field
                        v-model="inputData.modeloInversor"
                        :rules="[rules.required]"
                        label="Modelo Inversor"
                        class="mr-4 required"
                        style="width: 38%"
                        clearable
                        :loading="loading"
                        :disabled="disabledFields"
                    ></v-text-field>

                    <v-text-field
                        v-model="inputData.potenciaInversor"
                        :rules="[rules.required]"
                        @keypress="filtrarCaracterNumerico($event)"
                        label="Potência Inversor (KWP)"
                        class="mr-4 required"
                        style="width: 38%"
                        clearable
                        :loading="loading"
                        :disabled="disabledFields"
                    ></v-text-field>
                </div>

                <div class="d-flex mb-2" style="width: 100%">

                    <v-text-field
                        v-model="inputData.fabricanteModulo"
                        label="Fabricante Módulo"
                        class="mr-4 required"
                        :rules="[rules.required]"
                        style="width: 30%"
                        clearable
                        :loading="loading"
                        :disabled="disabledFields"
                    ></v-text-field>

                    <v-text-field
                        v-model="inputData.modeloModulo"
                        label="Modelo Módulo"
                        :rules="[rules.required]"
                        class="mr-4 required"
                        style="width: 30%"
                        :loading="loading"
                        clearable
                        :disabled="disabledFields"
                    ></v-text-field>

                    <v-text-field
                        v-model="inputData.quantidadeModulo"
                        :rules="[rules.required]"
                        validate-on-blur
                        label="Quantidade Módulo"
                        @keypress="filtrarCaracterNumerico($event)"
                        style="width: 30%"
                        hint="Somente números"
                        class="mr-4 required"
                        clearable
                        :loading="loading"
                        :disabled="disabledFields"
                    ></v-text-field>

                    <v-text-field
                        v-model="inputData.potenciaModulo"
                        :rules="[rules.required]"
                        label="Potência Módulo (KWP)"
                        style="width: 30%"
                        hint="Somente números"
                        @keypress="filtrarCaracterNumerico($event)"
                        class="mr-4 required"
                        validate-on-blur
                        clearable
                        :loading="loading"
                        :disabled="disabledFields"
                    ></v-text-field>

                </div>

                <div class="d-flex mb-2" style="width: 100%">

                    <v-select
                        item-value="value"
                        item-text="text"
                        :items="tipoGeracaoEnum"
                        v-model="inputData.tipoGeracao"
                        :rules="[rules.required]"
                        label="Tipo Geração"
                        class="mr-4 required"
                        style="width: 30%"
                        :menu-props="{ bottom: true, offsetY: true }"
                        clearable
                        :disabled="disabledFields"
                    ></v-select>

                    <v-select
                        item-value="value"
                        item-text="text"
                        :items="tipoAterramentoEnum"
                        v-model="inputData.tipoAterramento"
                        :rules="[rules.required]"
                        label="Tipo Aterramento"
                        class="mr-4 required"
                        style="width: 30%"
                        :menu-props="{ bottom: true, offsetY: true }"
                        clearable
                        :disabled="disabledFields"
                    ></v-select>

                    <v-text-field
                        v-model="inputData.numeroPosteUc"
                        outline
                        :loading="loading"
                        style="width: 30%"
                        class="mr-4"
                        label="Número Poste UC"
                        clearable
                        :disabled="disabledFields"
                    />

                    <v-text-field
                        v-model="inputData.numeroTransformador"
                        outline
                        :loading="loading"
                        style="width: 30%"
                        class="mr-4"
                        label="Número Transformador"
                        clearable
                        :disabled="disabledFields"
                    />
                    
                </div>
                <div class="d-flex mb-2" style="width: 100%">

                    <v-text-field
                        v-show="inputData.tipoGeracao === 2"
                        v-model="inputData.numeroUc1"
                        outline
                        :loading="loading"
                        style="width: 40%"
                        class="mr-4"
                        label="Número UC (1)"
                        clearable
                        :disabled="disabledFields"
                    />

                    <v-text-field
                        v-show="inputData.tipoGeracao === 2"
                        v-model="inputData.percentualUc1"
                        outline
                        :loading="loading"
                        style="width: 30%"
                        class="mr-4"
                        label="UC % (1)"
                        clearable
                        :disabled="disabledFields"
                    />

                    <v-text-field
                        v-show="inputData.tipoGeracao === 2"
                        v-model="inputData.numeroUc2"
                        outline
                        :loading="loading"
                        style="width: 30%"
                        class="mr-4"
                        label="Número UC (2)"
                        clearable
                        :disabled="disabledFields"
                    />

                    <v-text-field
                        v-show="inputData.tipoGeracao === 2"
                        v-model="inputData.percentualUc2"
                        outline
                        :loading="loading"
                        style="width: 40%"
                        class="mr-4"
                        label="UC % (2)"
                        clearable
                        :disabled="disabledFields"
                    />
                </div>

                <div class="d-flex" style="width: 100%">
                    <v-text-field
                        v-model="inputData.latitude"
                        style="width: 30%"
                        label="Latitude"
                        class="mr-4"
                        hint="Somente Números"
                        @keypress="filtrarCaracterNumerico($event)"
                        clearable
                        :loading="loading"
                        :disabled="disabledFields"
                    />

                    <v-text-field
                        v-model="inputData.longitude"
                        style="width: 30%"
                        class="mr-4"
                        label="Longitude"
                        hint="Somente Números"
                        @keypress="filtrarCaracterNumerico($event)"
                        clearable
                        :loading="loading"
                        :disabled="disabledFields"
                    />

                </div>
                <div class="d-flex mb-4" style="width: 60%">

                    <calendario
                        label="Data Criação"
                        :ruleIndexProps="2"
                        class="mr-4"
                        :disabled="true"
                        :dataProps="inputData.dataCriacao"
                        :required="false"
                        @calendarioDate="inputData.dataCriacao=$event"
                        style="width: 40%"
                    ></calendario>

                    <calendario
                        label="Data Homologação"
                        :ruleIndexProps="2"
                        :dataProps="inputData.dataHomologacao"
                        :required="false"
                        style="width: 40%"
                        :disabled="true"
                        @calendarioDate="inputData.dataHomologacao=$event"
                    ></calendario>
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

const moduleProjeto = createNamespacedHelpers("ModuleProjeto");
const moduleFase = createNamespacedHelpers("ModuleFase");
const moduleConcessionaria = createNamespacedHelpers("ModuleConcessionaria");


export default {
    props: {
        loading: {type: Boolean, default: false},
        disabledFields: {type: Boolean, default: false}
    },
    data: () => ({
        inputData:{
            id: 0,
            faseId: null,
            concessionariaId: null,
            protocolo: null,
            fabricanteInversor: null,
            modeloInversor: null,
            potenciaInversor: null,
            fabricanteModulo: null,
            modeloModulo: null,
            quantidadeModulo: null,
            potenciaModulo: null,
            tipoGeracao: null,
            tipoAterramento: null,
            numeroPosteUc: null,
            numeroTransformador: null,
            numeroUc1: null,
            percentualUc1: null,
            numeroUc2: null,
            percentualUc2: null,
            latitude: null,
            longitude: null,
            dataHomologacao: null,
            dataCriacao: null,
        },
        tenantId: null,
        fases: [],
        concessionarias: [],
        alertType: null,
        alertText: '',
        alertShow: false,
    }),
    
    components:{
        Calendario
    },

    computed: {
        fase: {
            get() {
                return +this.inputData.faseId
            },
            set(newValue) {
                this.inputData.faseId = newValue;
            }
        },
        concessionaria: {
            get() {
                return +this.inputData.concessionariaId
            },
            set(newValue) {
                this.inputData.concessionariaId = newValue;
            }
        },
        tipoGeracaoEnum() {
            return this.$store.state.ModuleProjeto.tipoGeracaoEnum;
        },
        tipoAterramentoEnum() {
            return this.$store.state.ModuleProjeto.tipoAterramentoEnum;
        },
    },

    watch: {
    },

    methods: {
        filtrarCaracterNumerico,
        filtrarCaracterString,
        filtrarCaracterAlfanumericos,
        ...moduleProjeto.mapActions(["create", "update"]),
        ...moduleFase.mapActions(["getAllFases",]),
        ...moduleConcessionaria.mapActions(["getAllConcessionarias",]),
        ...mapActions(["updateTopBarItems"]),

        async getFases(){
            this.fases = await this.getAllFases();
        },
        async getConcessionarias(){
            this.concessionarias = await this.getAllConcessionarias();
        },

    },
    mounted() {
        this.getFases();
        this.getConcessionarias();
        this.tenantId = this.$store.state.Session.tenant ? this.$store.state.Session.tenant.id : null;
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