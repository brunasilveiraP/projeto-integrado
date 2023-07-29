<template>
    <div class="flex-grow-1 rounded-lg">
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

        <!--        :title="nome"-->
        <title-component ref="titleComponentRef" :title="nome" class="ml-1">
            <template v-slot:title-custon>
                <v-switch
                    :label="isActive === true ? 'Ativo' : 'Inativo'"
                    v-model="isActive"
                    class="ml-4"
                ></v-switch>
            </template>
        </title-component>

        <div>
            <div class="tab-header elevation-4 light-letters">
                <v-tabs background-color="primary" v-model="tab" dark>
                    <v-tab
                        v-for="item in tabItems"
                        :key="item.id"
                        class="tab-item-box"
                        style="flex: 1"
                    >
                        <p class="pa-0 pt-3">{{ item.title }}</p>
                    </v-tab>
                </v-tabs>
            </div>

            <v-tabs-items class="elevation-1 px-6" v-model="tab">
                <v-tab-item style="padding-bottom: 15vh">
                    <dados-parceiro-tab ref="dadosParceiroTab"/>
                </v-tab-item>
                <v-tab-item eager>
                    <tabela-preco-tab ref="tabelaPrecoTab"/>
                </v-tab-item>
            </v-tabs-items>
        </div>
    </div>
</template>

<script>
import {createNamespacedHelpers, mapActions} from "vuex";
import DadosParceiroTab from "@/views/parceiro/tab/dadosParceiroTab.vue";
import TitleComponent from "@/components/shared/titleComponent.vue";
import TabelaPrecoTab from "@/views/parceiro/tab/tabelaPrecoTab.vue";
import store from "@/store";

const moduleParceiro = createNamespacedHelpers(
    "ModuleParceiro"
);

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
            { id: 1, title: "Dados" },
            { id: 2, title: "Informações Financeiras" },
        ],
        inputData: {},
        inputDataTabelaPreco: {},
        id: null,
    }),

    components: {
        TabelaPrecoTab,
        DadosParceiroTab,
        TitleComponent
    },

    methods: {
        ...mapActions(["updateTopBarItems"]),
        ...mapActions("ModuleParceiro", ["create", "update", "getById", "existsCnpj"]),
        ...mapActions(["changeGlobalLoading"]),
        async carregarParceiro() {
            if (this.id) {
                await this.changeGlobalLoading(true);
                const response = await this.getById(this.id);
                this.inputData = response.data.result;

                this.$refs.tabelaPrecoTab.inputData.id = this.inputData.id;
                this.inputDataTabelaPreco.pagaTrt = this.inputData.pagaTrt;
                this.inputDataTabelaPreco.diasPagamento = this.inputData.diasPagamento;
                this.inputDataTabelaPreco.tabelasPreco = this.inputData.tabelasPreco;

                this.$refs.dadosParceiroTab.inputData = this.inputData;
                this.$refs.dadosParceiroTab.cepMascara = this.inputData.endereco ? this.inputData.endereco.cep : null;
                this.$refs.tabelaPrecoTab.inputData = this.inputDataTabelaPreco;

                this.nome = abp.utils.toPascalCase(response.data.result.fantasia)
                this.isActive = response.data.result.status;

                if (this.inputData.endereco == null) {
                    this.$refs.dadosParceiroTab.cepMascara = ""
                    this.inputData.endereco = {};
                }
                await this.changeGlobalLoading(false);
            }
        },
        updateParceiroActive() {
            this.$refs.dadosParceiroTab.inputData.status = this.isActive;
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
            if(this.$refs.dadosParceiroTab.$refs.form.validate() && this.$refs.tabelaPrecoTab.$refs.form.validate()) {
                await this.changeGlobalLoading(true);
                this.inputData = this.$refs.dadosParceiroTab.inputData;
                this.inputDataTabelaPreco = this.$refs.tabelaPrecoTab.inputData;
                this.inputData.pagaTrt = this.inputDataTabelaPreco.pagaTrt;
                this.inputData.diasPagamento = this.inputDataTabelaPreco.diasPagamento;
                this.inputData.tabelasPreco = this.inputDataTabelaPreco.tabelasPreco;
                if (this.inputData.id > 0) {
                    
                    if(this.isActive === false) this.inputData.status = 0;
                    else this.inputData.status = 1;
                    
                    let response = await this.update(this.inputData);
                    if (response.status === 200) {
                        await this.changeGlobalLoading(false);
                        this.goToListagem("Registro atualizado com sucesso!");

                    } else {
                        this.mensagemErro(response);
                    }
                } else {

                    const resultCnpj = await this.existsCnpj(this.inputData.cnpj);

                    if(resultCnpj){
                        await this.changeGlobalLoading(false);
                        this.mensagemErro("Este CNPJ já esta vinculado a um parceiro.");
                    }else{
                        let response = await this.create(this.inputData);
                        if (response.status === 200) {
                            await this.changeGlobalLoading(false);
                            this.goToListagem("Registro cadastrado com sucesso!");
                        } else {
                            await this.changeGlobalLoading(false);
                            this.mensagemErro(response);
                        }
                    }
                }
            }else{
                this.mensagemErro("Preencha os campos obrigatórios");
            }
        },

        goToListagem(msg) {
            this.$router.push({name: "ViewParceiro"})
                .then(
                    store.dispatch(
                        'ModuleAlert/criarAlerta',
                        {message: msg, type: 'success',  displayAlert: true }
                    ));
        },

    },

    mounted() {
        this.buildTabInfo();
        this.id = this.$route.params.id;
        this.carregarParceiro();
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