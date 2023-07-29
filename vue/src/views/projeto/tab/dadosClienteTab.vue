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
                    <v-select
                        item-value="value"
                        item-text="text"
                        :items="tipoPessoaEnum"
                        v-model="inputData.tipoPessoa"
                        label="Tipo Pessoa"
                        class="mr-4 required"
                        style="width: 30%"
                        :menu-props="{ bottom: true, offsetY: true }"
                        clearable
                        :disabled="disabledFields"
                    ></v-select>

                    <v-text-field
                        v-model="inputData.cpf"
                        v-show="inputData.tipoPessoa === 1"
                        :rules="[cpf]"
                        label="CPF"
                        persistent-hint
                        hint="Somente números"
                        style="width: 30%"
                        class="mr-4"
                        clearable
                        validate-on-blur
                        :loading="fieldLoading"
                        :menu-props="{ bottom: true, offsetY: true }"
                        :disabled="disabledFields"
                    ></v-text-field>

                    <v-text-field
                        v-model="inputData.cnpj"
                        v-show="inputData.tipoPessoa === 2"
                        :rules="[cnpj]"
                        label="CNPJ"
                        persistent-hint
                        hint="Somente números"
                        style="width: 30%"
                        class="mr-4"
                        clearable
                        validate-on-blur
                        :loading="fieldLoading"
                        :menu-props="{ bottom: true, offsetY: true }"
                        :disabled="disabledFields"
                    ></v-text-field>

                    <v-text-field
                        v-model="inputData.telefone"
                        :rules="[rules.telefone]"
                        validate-on-blur
                        label="Telefone"
                        style="width: 15%"
                        hint="Somente números"
                        class="mr-4"
                        clearable
                        :loading="fieldLoading"
                        :disabled="disabledFields"
                    ></v-text-field>
                </div>

                <div class="d-flex" style="width: 100%">
                        <v-text-field
                            class="mr-4 required"
                            v-model="inputData.nome"
                            :rules="[rules.required]"
                            label="Nome"
                            style="width: 30%"
                            clearable
                            validate-on-blur
                            :loading="fieldLoading"
                            :disabled="disabledFields"
                        />
                        
                        <v-text-field
                            class="mr-4"
                            v-model="inputData.email"
                            :rules="[rules.email]"
                            label="E-mail"
                            style="width: 30%"
                            clearable
                            validate-on-blur
                            :loading="fieldLoading"
                            :disabled="disabledFields"
                        />
                </div>
            </div>

            <v-subheader class="d-flex align-center black--text textBold">Endereço</v-subheader>

            <div class="d-flex" style="width: 100%">

                <v-text-field
                    v-model="cepMascara"
                    outline
                    :rules="[rules.cep]"
                    :loading="fieldLoading"
                    style="width: 20%"
                    class="mr-4"
                    :maxlength="9"
                    label="CEP"
                    clearable
                    :disabled="disabledFields"
                />

                <v-text-field
                    v-model="inputData.endereco.rua"
                    outline
                    :loading="fieldLoading"
                    style="width: 40%"
                    :rules="[rules.required]"
                    class="mr-4 required"
                    label="Logradouro"
                    clearable
                    :disabled="disabledFields"
                />

                <v-text-field
                    v-model="inputData.endereco.numero"
                    style="width: 12%"
                    @keypress="filtrarCaracterNumerico($event)"
                    class="mr-4 required"
                    :rules="[rules.required]"
                    label="Número"
                    clearable
                    :loading="fieldLoading"
                    :disabled="disabledFields"
                />
            </div>

            <div class="d-flex" style="width: 100%">
                <v-text-field
                    v-model="inputData.endereco.complemento"
                    style="width: 30%"
                    label="Complemento"
                    class="mr-4"
                    clearable
                    :loading="fieldLoading"
                    :disabled="disabledFields"
                />

                <v-text-field
                    v-model="inputData.endereco.bairro"
                    style="width: 45%"
                    class="mr-4 required"
                    label="Bairro"
                    :rules="[rules.required]"
                    clearable
                    :loading="fieldLoading"
                    :disabled="disabledFields"
                />

                <v-text-field
                    v-model="inputData.endereco.cidade"
                    style="width: 35%"
                    class="mr-4 required"
                    label="Cidade"
                    :rules="[rules.required]"
                    clearable
                    :loading="fieldLoading"
                    :disabled="disabledFields"
                />

                <v-text-field
                    v-model="inputData.endereco.uf"
                    style="width: 20%"
                    label="UF"
                    class="mr-4 required"
                    :maxlength="2"
                    :rules="[rules.required]"
                    :loading="fieldLoading"
                    clearable
                    :disabled="disabledFields"
                />
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
import {cepMask, cpfMask, cnpjMask, telefoneMask} from "@/lib/masks";

const moduleProjeto = createNamespacedHelpers("ModuleProjeto");

export default {
    components:{
    },
    props: {
        loading: {type: Boolean, default: false},
        disabledFields: {type: Boolean, default: false}
    },
    data: () => ({
        fieldLoading: false,
        cepMascara: null,
        inputData: {
            id: 0,
            cnpj: null,
            cpf: null,
            nome: null,
            telefone: null,
            email: null,
            tipoPessoa: 1,
            endereco: {
                id: null,
                cep: null,
                rua: null,
                numero: null,
                complemento: null,
                bairro: null,
                cidade: null,
                uf: null
            },
        },
    }),

    methods: {
        filtrarCaracterNumerico,
        filtrarCaracterString,
        filtrarCaracterAlfanumericos,
    },
    mounted() {
    },

    computed: {
        tipoPessoaEnum() {
            return this.$store.state.ModuleProjeto.tipoPessoa;
        },
    },

    watch: {
        async "inputData.telefone"(telefone) {
            if(telefone){
                await this.$nextTick();
                this.inputData.telefone = telefoneMask(telefone);
            }
        },
        cepMascara(novoCep) {
            if(novoCep){
                this.cepMascara = cepMask(novoCep);
                this.inputData.endereco.cep = this.cepMascara;
            }
        },
        async "inputData.cpf"(cpf) {
            if(cpf){
                await this.$nextTick();
                this.inputData.cpf = cpfMask(cpf);
                this.inputData.cnpj = "";
            }
        },
        async "inputData.cnpj"(newCnpj) {
            if(newCnpj) {
                await this.$nextTick();
                this.inputData.cnpj = cnpjMask(newCnpj);
                this.inputData.cpf = "";
            }
        },
        "inputData.tipoPessoa"(tipoPessoa) {
            if(!tipoPessoa) this.inputData.tipoPessoa = 1
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