<template>
    <v-dialog v-model="value" persistent max-width="1000px">
        <div class="white pa-8 d-flex flex-column">

            <div class="d-flex flex-row justify-space-between">
                <h2 class="bbl-neutral--text">{{!editId ? 'Nova Tabela de Preço' : 'Edição Tabela de Preço' }}</h2>
                <v-btn text small fab @click="closeModal">
                    <v-icon>mdi-close</v-icon>
                </v-btn>
            </div>
            <hr class="mb-6" />

            <v-alert
                dismissible
                type="error"
                class="mt-2"
                v-show="alertErrorModal"
                transition="scroll-y-transition"
            >
                {{ alertErrorModalText }}
            </v-alert>

            <div class="my-4 d-flex flex-row">

                <v-form
                    ref="form"
                    v-model="valid"
                    class="d-flex flex-row"
                    style="flex-wrap: wrap; width: 100%"
                >
                    <v-autocomplete
                        v-model="inputData.kwpInicial"
                        class="input-box required mr-8"
                        label="Kwp Inicial"
                        clearable
                        style="width: 8%"
                        :items="getKwp"
                        :menu-props="{ bottom: true, offsetY: true }"
                    />

                    <span class="mt-6 mr-8"> a </span>

                    <v-autocomplete
                        v-model="inputData.kwpFinal"
                        class="input-box required mr-4"
                        label="Kwp Final"
                        clearable
                        style="width: 8%"
                        :items="getKwp"
                        :menu-props="{ bottom: true, offsetY: true }"
                    />
                    <Money
                        v-model="inputData.valor"
                        :label="$L('Valor')"
                        :properties="{
                              prefix: 'R$',
                              readonly: false,
                              disabled: false,
                              outlined: false,
                              dense: false,
                              clearable: true,
                            }"
                        :options="{
                              length: 12,
                              precision: 2,
                              empty: 0,
                            }"
                        class="mr-2 mt-4 required"
                        style="width: 40%"
                    />

                </v-form>
            </div>

            <div style="text-align: right">
                <v-btn
                    class="bbl-orange white--text"
                    @click="salvar"
                    :loading="loading"
                >
                    Salvar
                </v-btn>
            </div>
        </div>
    </v-dialog>

</template>

<script>

import { createNamespacedHelpers, mapActions } from 'vuex'
import ModalController from "@/mixins/ModalController";
import Money from "@/components/shared/Money.vue";

const moduleTabelaPreco = createNamespacedHelpers("ModuleTabelaPreco");

export default {

    props:[
        "editId"
    ],

    components:{
        Money
    },

    name: 'modalTabelaPreco',

    data: () => ({
        inputData: {
            id: 0,
            kwpInicial: null,
            kwpFinal: null,
            valor: null
        },
        loading: false,
        alertErrorModal : false,
        alertErrorModalText: '',
        valorInicial: null,
        valorFinal: null,
    }),
    methods: {
        ...moduleTabelaPreco.mapActions([ "getAll", "create", "getById", "update" ]),
        ...mapActions("ModuleParceiro", ["gerarKwp"]),

        async salvar(){
            this.loading = true;
            let response;

            if(this.editId){
                response = await this.update(this.inputData);
            }else{
                response = await this.create(this.inputData);
            }
            response = response.hasOwnProperty("status") ? response : response.response;
            this.loading = false;

            if (response.status === 200){
                this.$emit("atualizaTabela");

                this.editId ?
                    this.$emit("mensagemSucesso", "Registro atualizado com sucesso!") :
                    this.$emit("mensagemSucesso", "Registro cadastrado com sucesso!")

                this.closeModal();
            } else {
                this.alertErrorModal = true;
                this.alertErrorModalText = response;
                setTimeout(() => {
                    this.alertErrorModal = false;
                }, 5000);
            }
        },

        async setInputData(){
            if (this.editId) {
                this.getById(this.editId).then((result) => {
                    if (result.status === 200) {
                        this.inputData = result.data.result;
                    }else{
                        this.mensagemErro("Ocorreu um erro ao realizar esta ação. Contate o administrador do sistema.");
                    }
                });
            }
            else {
                this.inputData = {
                    id: 0,
                    nome: null,
                    valor: null
                }
            }
        }
    },
    mounted() {
        this.gerarKwp();
    },

    watch: {
        value() {
            this.setInputData();
        },

    },
    computed: {

        getKwp() {
            return this.$store.state.ModuleParceiro.kwp;
        },
    },

    mixins: [
        ModalController
    ],
};

</script>

<style scoped>

</style>