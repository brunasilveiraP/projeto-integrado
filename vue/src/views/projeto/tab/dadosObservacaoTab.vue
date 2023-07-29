<template>
    <div class="flex-grow-1 pa-4" style="width: 100%">

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

        <div class="d-flex flex-column flex-grow-1 rounded-lg white" style="width: 100%; height: 210px">
            <v-form lazy-validation ref="form">
                <div class="">
                    <h3 class="bbl">Nova Observação</h3>
                    <div class="mt-1 containerTexto">
                        <v-textarea
                            v-model="inputData.descricao"
                            clearable
                            solo
                            rows="3"
                            row-height="30"
                            no-resize
                            :counter="300"
                            maxlength="300"
                            class="textarea"
                            label="Digite sua observação aqui"
                        >
                            <template v-slot:label>
                                <div class="texto">
                                    <v-avatar class="bbl-orange" size="30">
                                        <span style="" class="white--text text-h8">{{iniciais}}</span>
                                    </v-avatar>
                                    <span>Digite sua observação aqui</span>
                                </div>
                            </template>
                        </v-textarea>
                    </div>

                    <div style="text-align: right">
                        <v-btn
                            class="bbl-orange white--text pr-8 pl-8"
                            :loading="loading"
                            @click="handleCreateObservacao"
                        >Salvar
                        </v-btn>
                    </div>
                </div>
            </v-form>
        </div>

        <div
            class="d-flex flex-column mr-4 pa-4 mt-2 flex-grow-1 rounded-lg white elevation-1"
            style="width: 100%;"
        >
            <div class="d-flex justify-space-between mb-4">
                <h3 class="bbl">Observações Recentes</h3>
            </div>
            <div style="height: 250px; overflow-y: auto;">
                <div
                    class="d-flex pa-2 align-center"
                    style="width: 100%;"
                    :key="item.id"
                    v-for="item in observacoes"
                    v-bind:class="{ active: item.fixed == true }"
                >
                    <div class="d-flex align-center justify-center" style="height: 100%;">
                    <v-avatar class="bbl-orange mr-4" size="30">
                            <span class="white--text text-h8">{{ iniciais }}</span>
                        </v-avatar>
                    </div>

                    <p style="width: 100%; word-break: break-word">
                        {{ item.descricao }}
                    </p>

                    <v-fab-transition class="d-flex">
                        <v-btn  text fab x-small bottom>
                            <v-icon @click="fixarMensagemSelecionada(item)" v-show="item.fixed"
                            >mdi-pin-off</v-icon
                            >
                            <v-icon @click="fixarMensagemSelecionada(item)" v-show="!item.fixed"
                            >mdi-pin</v-icon
                            >
                        </v-btn>
                    </v-fab-transition>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import {mapActions} from "vuex";
import validationRules from "@/mixins/validationRules.js";

export default {
    mixins: [validationRules],
    props: {
    },
    data() {
        return {
            iniciais: null,
            loading: false,
            alertType: null,
            alertText: '',
            alertShow: false,
            inputData: {
                projetoId: null,
                fixed: false,
                descricao: null,
            },
            isFixed: false,
            observacoes: [],
        };
    },

    mounted() {
        this.carregarMensagens();
        this.carregarAvatar();
    },

    methods: {
        ...mapActions("ModuleProjetoObservacao", [
            "create",
            "update",
            "getAllObservacoes",
        ]),

        carregarAvatar() {
            const userName = this.$store.state.Session.user ? this.$store.state.Session.user.name.toUpperCase() : "";
            const array = userName.split(" ");
            let iniciais = "";
            if (array) {
                iniciais = `${array[0].substr(0, 1)} ${array[array.length - 1].substr(
                    0,
                    1
                )}`;
            }
            this.iniciais = iniciais;

        },
        async handleCreateObservacao() {
            if (this.$refs.form.validate()) {
                this.loading = true;
                this.inputData.projetoId = this.$route.params.id;
                const response = await this.create(this.inputData);
                this.mensagemSucesso("Observação adicionada com sucesso.")
                if (response.status === 200) {
                    await this.carregarMensagens();
                    this.resetObservacao();
                }else{
                    this.mensagemErro("Ocorreu um erro ao realizar esta ação. Contate o administrador do sistema.");
                }
                this.loading = false;
            }
        },
        resetObservacao() {
            this.inputData = {
                fixed: false,
                descricao: null,
            };
            this.loading = false;
        },
        async carregarMensagens() {
            const response = await this.getAllObservacoes({
                skipCount: 0,
                itemsPerPage: 1000,
                projetoId: this.$route.params.id,
            });
            if (response.status === 200) {
                this.observacoes = response.data.result.items;
            }

            this.ordenarListaPorComentario();
        },
        ordenarListaPorComentario() {
            var comentario = this.observacoes;
            if (comentario != null) {
                var lista = [];
                var item = {};
                comentario.forEach((e) => {
                    if (e.fixed === true) item = e;
                });
                if (item["id"]) lista.push(item);

                comentario.forEach((e) => {
                    if (item != null) {
                        if (e.id != item.id) lista.push(e);
                    } else {
                        lista.push(e);
                    }
                });
                this.observacoes = lista;
            }
        },

        async fixarMensagemSelecionada(item) {
            if (!item.fixed && this.verificarMensagemFixadaExistente()) {
                this.mensagemErro("Já existe um comentário fixado. Verifique as informações.")
                return;
            }

            let fixed = item.fixed;
            item.fixed = !fixed;
            let message = !fixed ? "Comentário fixado com sucesso" : "Comentário desafixado com sucesso"
            const response = await this.update(item);
            this.mensagemSucesso(message);
            this.carregarMensagens();
        },

        verificarMensagemFixadaExistente() {
            let isItemFixed = false;
            this.observacoes.forEach(function (item) {
                if (item.fixed) isItemFixed = true;
            });
            return isItemFixed;
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
    },
};
</script>

<style scoped>
.active {
    background: #e6e6e6;
    height: 50px;
    width: 50px;
}
.containerTexto {
    display: flex;
    position: relative;
    justify-content: left;
}

.textarea {
    position: relative;
    align-items: center;
    justify-content: center;
}

.textarea >>> .v-label {
    height: auto;
    top: 25px !important;
}

.texto {
    display: flex;
    justify-content: center;
    align-items: center;
    gap: 5px;
}

.v-application p {
    margin-bottom: 0px !important;
}

.fixed {
    background: "#E6E6E6";
}

.noFixed {
    background: "#fff";
}
</style>
