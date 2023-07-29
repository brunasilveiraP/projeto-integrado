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

                <v-subheader class="d-flex align-center black--text textBold">Dados Empresariais</v-subheader>
                <div class="d-flex" style="width: 100%">
                    <v-text-field
                        v-model="inputData.cnpj"
                        :rules="[rules.required]"
                        label="CNPJ"
                        persistent-hint
                        hint="Somente números"
                        class="mr-4 required"
                        style="width: 30%"
                        clearable
                        validate-on-blur
                        :loading="fieldLoading"
                        :menu-props="{ bottom: true, offsetY: true }"
                    />

                    <v-text-field
                        v-model="inputData.razaoSocial"
                        :rules="[rules.required]"
                        @keypress="filtrarCaracterAlfanumericos($event)"
                        label="Razão Social"
                        class="mr-4 required"
                        style="width: 38%"
                        :loading="fieldLoading"
                        clearable
                    />

                    <v-text-field
                        v-model="inputData.fantasia"
                        :rules="[rules.required]"
                        label="Fantasia"
                        class="mr-4 required"
                        style="width: 38%"
                        clearable
                        :loading="fieldLoading"
                    />
                </div>

                <div class="d-flex" style="width: 100%">

                    <v-text-field
                        v-model="inputData.inscricaoEstadual"
                        @keypress="filtrarCaracterNumerico($event)"
                        label="Inscrição Estadual"
                        hint="Somente números"
                        class="mr-4"
                        style="width: 20%"
                        clearable
                        :loading="fieldLoading"
                    />

                    <v-text-field
                        v-model="inputData.inscricaoMunicipal"
                        @keypress="filtrarCaracterNumerico($event)"
                        label="Inscrição Municipal"
                        hint="Somente números"
                        class="mr-4"
                        style="width: 20%"
                        :loading="fieldLoading"
                        clearable
                    />

                    <v-text-field
                        v-model="inputData.telefone1"
                        :rules="[rules.telefone, rules.required]"
                        validate-on-blur
                        label="Telefone 1"
                        style="width: 15%"
                        hint="Somente números"
                        class="mr-4 required"
                        clearable
                        :loading="fieldLoading"
                    />

                    <v-text-field
                        v-model="inputData.telefone2"
                        :rules="[rules.telefone]"
                        label="Telefone 2"
                        style="width: 15%"
                        hint="Somente números"
                        class="mr-4"
                        validate-on-blur
                        clearable
                        :loading="fieldLoading"
                    />

                </div>

                <div class="d-flex" style="width: 100%">
                    <div class="d-flex" style="width: 50%">
                        <v-text-field
                            class="mr-4 required"
                            v-model="inputData.email"
                            :rules="[rules.required, rules.email]"
                            label="E-mail"
                            persistent-hint
                            hint="Receberá notificações sobre atualizações dos projetos"
                            style="width: 40%"
                            clearable
                            validate-on-blur
                            :loading="fieldLoading"
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
                        :maxlength="9"
                        style="width: 20%"
                        class="mr-4"
                        label="CEP"
                        clearable
                    />

                    <v-text-field
                        v-model="inputData.endereco.rua"
                        outline
                        :loading="fieldLoading"
                        style="width: 40%"
                        class="mr-4"
                        label="Logradouro"
                        clearable
                    />

                    <v-text-field
                        v-model="inputData.endereco.numero"
                        style="width: 12%"
                        @keypress="filtrarCaracterNumerico($event)"
                        class="mr-4"
                        label="Número"
                        clearable
                        :loading="fieldLoading"
                    />
                </div>

                <div class="d-flex" style="width: 100%">
                    <v-text-field
                        v-model="inputData.endereco.complemento"
                        style="width: 30%"
                        label="Complemento"
                        class="mr-4"
                        @keypress="filtrarCaracterAlfanumericos($event)"
                        clearable
                        :loading="fieldLoading"
                    />

                    <v-text-field
                        v-model="inputData.endereco.bairro"
                        style="width: 45%"
                        class="mr-4"
                        label="Bairro"
                        clearable
                        :loading="fieldLoading"
                    />

                    <v-text-field
                        v-model="inputData.endereco.cidade"
                        style="width: 35%"
                        class="mr-4"
                        label="Cidade"
                        clearable
                        :loading="fieldLoading"
                    />

                    <v-text-field
                        v-model="inputData.endereco.uf"
                        style="width: 20%"
                        label="UF"
                        :maxlength="2"
                        @keypress="filtrarCaracterString($event)"
                        :loading="fieldLoading"
                        clearable
                    />
                </div>
            </div>
        </v-form>
    </div>
</template>

<script>
import validationRules from "@/mixins/validationRules.js";
import { mapActions,createNamespacedHelpers } from "vuex";
import {
    cnpjMask,
    telefoneMask,
    cepMask
} from "@/lib/masks.js";
import {
    filtrarCaracterNumerico,
    filtrarCaracterString,
    filtrarCaracterAlfanumericos,
} from "@/lib/helpers/validarCaracteresPermitidos.js";

const moduleParceiro = createNamespacedHelpers(
    "ModuleParceiro"
);

export default {
    data: () => ({
        fieldLoading: false,
        cepMascara: null,
        inputData:{
            id: 0,
            cnpj: null,
            razaoSocial: null,
            fantasia: null,
            inscricaoMunicipal: null,
            inscricaoEstadual: null,
            telefone1: null,
            telefone2: null,
            email: null,
            status: true,
            enderecoId: 0,
            endereco: {
                id: 0,
                cep: null,
                rua: null,
                numero: null,
                complemento: null,
                bairro: null,
                cidade: null,
                uf: null
            },
        },
        alertType: null,
        alertText: '',
        alertShow: false,
    }),

    watch: {
        async "inputData.telefone1"(telefone) {
            if(telefone){
                await this.$nextTick();
                this.inputData.telefone1 = telefoneMask(telefone);
            }
        },
        async "inputData.telefone2"(telefone) {
            if(telefone) {
                await this.$nextTick();
                this.inputData.telefone2 = telefoneMask(telefone);
            }
        },
        async "inputData.cnpj"(newCnpj) {
            if(newCnpj){
                await this.$nextTick();
                this.inputData.cnpj = cnpjMask(newCnpj);
            }
        },
        cepMascara(novoCep) {
            if(novoCep){
                this.cepMascara = cepMask(novoCep);
                this.inputData.endereco.cep = this.cepMascara;
            }
        },
    },

    methods: {
        filtrarCaracterNumerico,
        filtrarCaracterString,
        filtrarCaracterAlfanumericos,
        ...moduleParceiro.mapActions(["create", "update"]),
        ...mapActions(["updateTopBarItems"]),
        
    },

    mixins: [
        validationRules,
    ],
};
</script>

<style scoped>

</style>