<template>
    <v-dialog v-model="display" max-width="600px">
        <div class="white pa-8 d-flex flex-column">
            <div class="d-flex flex-row justify-space-between mb-3">
                <h2>Senha</h2>
                <v-btn text small fab @click="closeModal">
                    <v-icon>mdi-close</v-icon>
                </v-btn>
            </div>

            <v-alert
                dismissible
                type="error"
                class="mt-2"
                v-show="alertErrorModal"
                transition="scroll-y-transition"
            >
                {{ alertErrorModalText }}
            </v-alert>

            <div class="mt-5">
                <v-form ref="form" v-model="valid">
                    <v-text-field
                        :append-icon="showPass ? 'mdi-eye' : 'mdi-eye-off'"
                        :type="showPass ? 'text' : 'password'"
                        @click:append="showPass = !showPass"
                        label="Insira a nova senha"
                        class="mb-5 pt-2 input-none-case"
                        style="margin-top: -20px; font-size: 14px"
                        v-model="password"
                        :rules="[rules.required]"
                        clearable
                    ></v-text-field>

                    <v-text-field
                        :append-icon="showPassConfirm ? 'mdi-eye' : 'mdi-eye-off'"
                        :type="showPassConfirm ? 'text' : 'password'"
                        @click:append="showPassConfirm = !showPassConfirm"
                        class="mt-5 input-none-case"
                        label="Confirme a nova senha"
                        style="margin-top: -20px; font-size: 14px"
                        v-model="confirmPassword"
                        :rules="[rules.required]"
                        clearable
                    ></v-text-field>
                </v-form>
            </div>

            <div style="text-align: right">
                <v-btn class="bbl-orange white--text" @click="submitForm"
                >Salvar</v-btn
                >
            </div>
        </div>
    </v-dialog>
</template>

<script>
import ValidationRules from "@/mixins/validationRules.js";
import { createNamespacedHelpers } from "vuex";
import ModalController from "@/mixins/ModalController";

const moduleAuthenticate = createNamespacedHelpers("ModuleAuthenticate");

export default {
    data: () => ({
        valid: true,
        inputData: {},
        confirmPassword: null,
        password: null,
        showPass: false,
        showPassConfirm: false,
        loading: false,
        alertErrorModal : false,
        alertErrorModalText: '',
    }),

    methods: {
        ...moduleAuthenticate.mapActions(["alterarSenha"]),

        async submitForm() {
            if (this.$refs.form.validate()) {
                if (this.password === this.confirmPassword) {
                    let response = await this.alterarSenha({
                        data: {
                            SenhaAtual: this.senhaAtual,
                            NovaSenha: this.password,
                        },
                    });
                    if (response.status == 400) {
                        this.alertErrorModal = true;
                        this.alertErrorModalText = "É necessário no mínimo 8 caracteres, 1 caracter especial, 1 letra maiúscula e números.";
                        setTimeout(() => {
                            this.alertErrorModal = false;
                        }, 7000);
                        
                    } else {
                        this.$emit("submit");
                        this.closeModal();
                    }
                } else {
                    this.alertErrorModal = true;
                    this.alertErrorModalText = "As senhas informadas não conferem.";
                    setTimeout(() => {
                        this.alertErrorModal = false;
                    }, 7000);
                }
            }
        },
    },
    props: ["display", "senhaAtual"],

    mixins: [ValidationRules, ModalController],
};
</script>