<template>
    <v-container fluid>
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

        <v-row class="white elevation-2 space-between mt-2 mx-0 align-center row-border">
            <v-col cols="4" align="start">
                <h2 class="bbl-blue--text">{{ nome }}</h2>
            </v-col>

            <v-col align="end" cols="4">
            <span>
                <strong>Valor do Projeto:  </strong>
                <v-btn
                    class="bbl-blue pa-2" dark
                >
                    {{ inputData.valor ? formatarMoedaMask(inputData.valor) : "Não calculado"}}
                </v-btn>
                </span>
            </v-col>
            <v-col align="end" cols="4">
                <span class="mr-2">
                    <strong>Status de Pagamento:  </strong>
                        <v-btn
                            class="bbl-blue pa-2" dark
                        >
                            {{ inputData.statusPagamento != null ?
                            this.$store.state.ModuleProjeto.statusPagamentoEnum.filter(item => item.value === inputData.statusPagamento)[0].text :
                            "Não faturado"}}
                        </v-btn>
                    </span>
            </v-col>
        </v-row>

        <v-row>
            <v-col>
                <v-card>

                    <v-tabs
                        background-color="primary"
                        center-active
                        dark
                        v-model="tab"
                    >
                        <v-tab
                            v-for="item in tabItems"
                            :key="item.id"
                            class="tab-item-box"
                            style="flex: 1"
                        >
                            <p class="tabName pa-0 pt-2">{{ item.title }}</p>
                        </v-tab>
                    </v-tabs>

                    <v-tabs-items class="elevation-1 px-3" v-model="tab">
                        <v-tab-item eager>
                            <dados-projeto-tab-v2 ref="dadosProjetoTab" :disabledFields="disabled"/>
                        </v-tab-item>
                        <v-tab-item eager>
                            <dados-cliente-tab ref="dadosClienteTab" :disabledFields="disabled"/>
                        </v-tab-item>
                        <v-tab-item>
                            <dados-anexo-tab ref="dadosAnexoTab" :projeto-id="id"/>
                        </v-tab-item>
                        <v-tab-item eager>
                            <dados-observacao-tab ref="dadosObservacaoTab"/>
                        </v-tab-item>
                    </v-tabs-items>

                </v-card>
            </v-col>
        </v-row>

    </v-container>
</template>

<script>
import {createNamespacedHelpers, mapActions} from "vuex";
import DadosProjetoTab from "@/views/projeto/tab/dadosProjetoTab.vue";
import TitleComponent from "@/components/shared/titleComponent.vue";
import DadosClienteTab from "@/views/projeto/tab/dadosClienteTab.vue";
import DadosAnexoTab from "@/views/projeto/tab/dadosAnexoTab.vue";
import DadosObservacaoTab from "@/views/projeto/tab/dadosObservacaoTab.vue";
import {formatarMoedaMask} from "@/lib/masks";
import store from "@/store";
import dadosProjetoTabV2 from "@/views/projeto/tab/dadosProjetoTab-v2.vue";
import DadosProjetoTabV2 from "@/views/projeto/tab/dadosProjetoTab-v2.vue";

const moduleProjeto = createNamespacedHelpers("ModuleProjeto");

export default {
    name: "viewManutencaoProjeto-v2",
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
            { id: 1, title: "Dados do Projeto" },
            { id: 2, title: "Dados do Cliente" },
            { id: 3, title: "Anexos" },
            { id: 4, title: "Observações" },
        ],
        inputData: {},
        inputDataCliente: {},
        id: null,
        disabled: false,
    }),

    components: {
        DadosClienteTab,
        DadosProjetoTab,
        DadosProjetoTabV2,
        DadosAnexoTab,
        DadosObservacaoTab,
        TitleComponent
    },

    methods: {
        ...mapActions(["updateTopBarItems"]),
        ...mapActions("ModuleProjeto", ["create", "update", "getById"]),
        ...mapActions(["changeGlobalLoading"]),
        formatarMoedaMask,

        async carregarProjeto() {
            if (this.id) {
                await this.changeGlobalLoading(true);
                const response = await this.getById(this.id);
                this.inputData = response.data.result;
                this.$refs.dadosProjetoTab.inputData = this.inputData;
                this.$refs.dadosClienteTab.inputData = this.inputData.cliente;

                if(this.inputData.cliente == null){
                    this.$refs.dadosClienteTab.inputData = {};
                    this.$refs.dadosClienteTab.inputData.endereco = {};
                    this.$refs.dadosClienteTab.inputData.tipoPessoa = 1;
                }else{
                    this.$refs.dadosClienteTab.cepMascara = this.inputData.cliente.endereco.cep;
                }

                this.nome = abp.utils.toPascalCase(this.inputData.parceiroNome)
                await this.changeGlobalLoading(false)

                if(this.inputData.statusPagamento === 2){
                    this.disabled = true;
                }
            }
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
            await this.updateTopBarItems([
                {
                    icon: "mdi-content-save",
                    nome: "Salvar",
                    clickFunction: () => this.submitForm(),
                    color: "primary",
                },
            ]);
        },
        async submitForm(){
            if(this.$refs.dadosProjetoTab.$refs.form.validate() && this.$refs.dadosClienteTab.$refs.form.validate()) {
                await this.changeGlobalLoading(true);
                this.inputData = this.$refs.dadosProjetoTab.inputData;
                this.inputData.cliente = this.$refs.dadosClienteTab.inputData;

                let response = await this.update(this.inputData);
                if (response.status === 200) {
                    this.goToListagem();
                } else {
                    this.mensagemErro(response);
                }
                await this.changeGlobalLoading(false);
            }else{
                this.mensagemErro("Preencha os campos obrigatórios");

            }
        },

        goToListagem() {
            this.$router.push({name: "ViewProjeto"})
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
        this.carregarProjeto();
    },
    mixins: [
    ],
};
</script>

<style scoped>

.row-border{
    border-radius: 6px;
}

</style>