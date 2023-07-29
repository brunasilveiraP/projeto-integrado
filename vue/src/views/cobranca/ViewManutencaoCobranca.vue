<template>
    <div class="flex-grow-1 rounded-lg ml-6 mr-6">
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

        <div
            class="d-flex flex-column white  my-2 elevation-1 rounded-sm"
            style="position: relative; z-index: 0"
        >
            <div class="d-flex justify-space-between px-5">
                <div class="d-flex align-baseline mt-5">
                    <h2 class="bbl-blue--text mr-12  mb-5" :loading="loading">{{ nome }}</h2>
                </div>
                <div class="d-flex align-baseline mt-5">
                    <v-btn class="bbl-blue hover-click" dark :loading="loading" @click="confirmarPagamentoCobranca">
                        Confirmar Pagamento
                        <v-icon class="ml-4">mdi-check-decagram</v-icon>
                    </v-btn>
                </div>
            </div>
        </div>

        <div class="">
            <div class="tab-header elevation-4 light-letters">
                <v-tabs class="tabela" background-color="primary" v-model="tab" dark>
                    <v-tab
                        v-for="item in tabItems"
                        :key="item.id"
                        class="tab-item-box"
                        style="flex: 1"
                    >
                        <p class="tabName pa-0 pt-3">{{ item.title }}</p>
                    </v-tab>
                </v-tabs>
            </div>

            <v-tabs-items class="elevation-1 px-6" v-model="tab">
                <v-tab-item style="padding-bottom: 15vh">
                    <dados-cobranca-tab ref="dadosCobrancaTab" :tenantId="inputData.tenantId"/>
                </v-tab-item>
            </v-tabs-items>
        </div>
    </div>
</template>

<script>
import {createNamespacedHelpers, mapActions} from "vuex";
import DadosCobrancaTab from "@/views/cobranca/tab/dadosCobrancaTab.vue";
import TitleComponent from "@/components/shared/titleComponent.vue";
import store from "@/store";

export default {
    data: () => ({
        rows: [],
        nome: null,
        isActive: true,
        alertType: null,
        alertText: '',
        alertShow: false,
        fieldLoading: false,
        tab: 0,
        tabItems: [
            { id: 1, title: "Dados de Cobrança" },
        ],
        inputData: {},
        id: null,
        loading: false,
    }),

    components: {
        DadosCobrancaTab,
        TitleComponent
    },

    methods: {
        ...mapActions(["updateTopBarItems"]),
        ...mapActions("ModuleCobranca", ["create", "update", "getById", "confirmarPagamento"]),
        ...mapActions(["changeGlobalLoading"]),
        async carregarCobranca() {
            if (this.id) {
                this.changeGlobalLoading(true);
                const response = await this.getById(this.id);
                this.inputData = response.data.result;
                this.$refs.dadosCobrancaTab.inputData = this.inputData;
                this.nome = abp.utils.toPascalCase(this.inputData.parceiroNome)
                this.changeGlobalLoading(false);
            }
        },

        async confirmarPagamentoCobranca(){
            this.changeGlobalLoading(true);
            this.inputData = this.$refs.dadosCobrancaTab.inputData;
                let response = await this.confirmarPagamento(this.inputData.id);
                if (response.status === 200) {
                    this.$refs.dadosCobrancaTab.inputData = response.data.result
                    this.mensagemSucesso("Registro atualizado com sucesso!");
                }else {
                    this.mensagemErro(response);
                }
            this.changeGlobalLoading(false);
        },

        mensagemSucesso(mensagem) {
            this.alertShow = true;
            this.alertType = "success";
            this.alertText = mensagem;
            setTimeout(() => {
                this.alertShow = false;
            }, 8000);
        },

        mensagemErro(mensagem) {
            this.alertShow = true;
            this.alertType = "error";
            this.alertText = mensagem;
            setTimeout(() => {
                this.alertShow = false;
            }, 8000);
        },
        async buildTabInfo() {
            this.updateTopBarItems([
                {
                    icon: "mdi-content-save",
                    nome: "Salvar",
                    clickFunction: () => this.submitForm(),
                    color: "primary",
                },
            ]);
        },
        async submitForm(){
            if(this.$refs.dadosCobrancaTab.$refs.form.validate()) {
                this.changeGlobalLoading(true);
                this.inputData = this.$refs.dadosCobrancaTab.inputData;
                if (this.inputData.id > 0) {
                    let response = await this.update(this.inputData);
                    if (response.status === 200) {
                        this.changeGlobalLoading(false);
                        this.goToListagem();
                    } else {
                        this.changeGlobalLoading(false);
                        this.mensagemErro(response);
                    }
                }
            }
        },
        goToListagem() {
            this.$router.push({name: "ViewCobranca"})
                .then(
                    store.dispatch(
                        'ModuleAlert/criarAlerta',
                        {message: "Registro salvo com sucesso!", type: 'success',  displayAlert: true }
                    ));
        },

    },

    mounted() {
        this.buildTabInfo();
        this.id = this.$route.params.id;
        this.carregarCobranca();
    },
    mixins: [
    ],
};
</script>

<style scoped>

.tab-header {
    width: 100%;
    background-color: #FF6A13;
    border-top-right-radius: 8px;
    border-top-left-radius: 8px;
    padding: 0 0 5px 5px;
}

</style>