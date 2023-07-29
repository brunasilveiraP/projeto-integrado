<template>
    <div class="wrapper-container d-flex flex-column">
        <v-container class="bbl-blue" fluid fill-height>

            <modal-criar-senha
                :display="displayModalCreatePass"
                @submit="submitCreatePass"
                :senhaAtual="this.inputData.password"
                @close="hideModal"
            ></modal-criar-senha>
            <modal-recuperar-senha
                :display="displayModalRecoveryPass"
                @submit="submitRecoveryPass($event)"
                @close="hideModal"
            ></modal-recuperar-senha>

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

            <v-card class="mt-4 mx-auto text-center hidden-sm-only elevation-24" min-width="320">
                <v-card-text>
                    <v-card class="v-card--offset mx-auto lighten" elevation="4" dark>
                        <v-card-text class="">
                            <div>
                                <img class="mt-5" src="@/assets/logo-login.png" alt="Logo da BBL" />
                            </div>
                        </v-card-text>
                        <div>
                            <v-card-text class="headline white--text font-weight-bold"
                                         style="font-family: 'Open Sans', sans-serif; font-size: 50px !important; letter-spacing: -6px !important;">
                                BBL
                            </v-card-text>
                            <v-card-text class="headline white--text font-weight-bold"
                                         style="font-family: 'Open Sans', sans-serif; 
                                             font-size: 30px !important; 
                                             letter-spacing: -1px !important; margin-top: -25px !important;">
                                Gestão
                            </v-card-text>
                        </div>
                    </v-card>
                </v-card-text>
                <v-form
                    class="d-flex flex-column"
                    ref="form"
                    v-model="valid">
                    <v-card-text>
                        <v-text-field
                            type="email"
                            class="mb-6"
                            label="Usuário"
                            :rules="[rules.required]"
                            v-model="inputData.userNameOrEmailAddress"
                            validate-on-blur
                            dense
                            @keypress.enter="submitAuthentication"
                            clearable
                        ></v-text-field>
                        <v-text-field
                            :rules="[rules.required]"
                            :append-icon="showPass ? 'mdi-eye' : 'mdi-eye-off'"
                            :type="showPass ? '' : 'password'"
                            @click:append="showPass = !showPass"
                            label="Senha"
                            v-model="inputData.password"
                            dense
                            @keypress.enter="submitAuthentication"
                            clearable
                        ></v-text-field>
                    </v-card-text>
                    <v-card-actions>
                        <v-row align="center" no-gutters>
                            <v-col class="text-center">
                                <div class="my-2">
                                    <v-btn
                                        block
                                        color="lighten"
                                        @click="submitAuthentication"
                                        :loading="loading"
                                    >Acessar</v-btn>
                                </div>
                                <div>
                                    <v-btn color="primary"
                                           x-small text
                                           @click="displayModal('modalRecoveryPass')"
                                    >
                                        Esqueceu sua senha?
                                    </v-btn>
                                </div>
                            </v-col>
                        </v-row>
                    </v-card-actions>
                </v-form>
            </v-card>
        </v-container>
    </div>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'
import ValidationRules from "@/mixins/validationRules";
import ModalCriarSenha from "@/views/auth/modal/modalCriarSenha.vue";
import ModalRecuperarSenha from "@/views/auth/modal/modalRecuperarSenha.vue";
import ModalController from "@/mixins/ModalController";
import Util from "@/lib/util";

const moduleAuthenticate = createNamespacedHelpers("ModuleAuthenticate");
const session = createNamespacedHelpers("Session");

export default {
    data: () => ({
        inputData: {
            userNameOrEmailAddress: '',
            password: '',
        },
        displayModalRecoveryPass: false,
        displayModalCreatePass: false,
        valid: true,
        showPass: false,
        loading: false,
        alertType: null,
        alertText: '',
        alertShow: false,
    }),
    components: {
        ModalCriarSenha,
        ModalRecuperarSenha
    },

    props: {
        senhaAtual: String,
    },
    methods: {
        ...moduleAuthenticate.mapActions(["authenticate","externalPasswordRecovery" ]),
        ...session.mapActions(["init","carregarPermissoes" ]),

        displayModal(type) {
            if (type === "modalRecoveryPass") {
                this.displayModalRecoveryPass = true;
            }

            if (type === "modalCreatePass") {
                this.displayModalCreatePass = true;
            }
        },

        hideModal() {
            this.displayModalRecoveryPass = false;
            this.displayModalCreatePass = false;
        },

        async submitAuthentication() {
            if (this.$refs.form.validate()) {
                this.loading = true;
                let response = await this.authenticate(this.inputData);
                if (response.status !== 200) {
                    this.mensagemErro("Nome de usuário ou senha inválido");
                    this.loading = false;
                }else{
                    if (this.$store.state.ModuleAuthenticate.checkedToChangePassword) {
                        this.loading = false;
                        this.displayModal("modalCreatePass");
                    } else await this.redirectTo();
                }
            }
        },

        async redirectTo() {
            await this.$router.push({path: "/app/administracao"});
            await this.init();
            const permissionsResponse = await this.carregarPermissoes();
            if (permissionsResponse) {
                Util.abp.auth.grantedPermissions = permissionsResponse.data.result
                location.reload()
            }
        },

        async submitRecoveryPass(e) {
            let response = await this.externalPasswordRecovery(e.email);
            if (response.status === 200) {
                this.hideModal();
                this.mensagemSucesso("E-mail de recuperação de senha encaminhado com sucesso!")
            } else {
                this.hideModal();
                this.mensagemErro("E-mail informado está incorreto ou não está cadastrado.");
            }
        },

        async submitCreatePass() {
            this.hideModal();
            this.inputData.password = null;
            this.mensagemSucesso("Senha criada com sucesso!");
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

    mixins: [ValidationRules, ModalController],
};
</script>

<style lang="scss" scoped>

.v-card--offset {
    top: -32px;
    position: relative;
}

</style>