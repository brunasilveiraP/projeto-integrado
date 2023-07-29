<template xmlns:v-slot="http://www.w3.org/1999/XSL/Transform">
    <v-container fluid>

        <modal-redefinicao-senha
            v-model="showModal"
            :usuarioId="inputData.id"
            ref="confirmRedefinirSenhaRef"
            @dispararMensagemSucesso="mensagemSucesso"
            @dispararMensagemErro="mensagemErro"
        />

        <alert-dialog
            title="Novo Usuário"
            titleEdit="Edição Usuário"
            v-model="showDialog"
            :inputData="inputData"
            :loading="loading"
            :edit-id="editId"
            :is-usuario="true"
            @showModalRecuperacao="showModal = $event"
            @submit="salvar"
        >
            <template v-slot:default>
                <v-row no-gutters class="pa-2 mt-2">
                    <v-col cols="12">
                        <v-form ref="form" v-model="valid">
                            <v-row no-gutters  class="">
                                <v-col>
                                    <v-text-field
                                        label="Nome"
                                        class="mr-4 required"
                                        v-model="inputData.name"
                                        :rules="[rules.required, rules.nameValidation]"
                                        clearable
                                        @keypress="filtrarCaracterString($event)"
                                    />
                                </v-col>
                                <v-col>
                                    <v-text-field
                                        label="Login"
                                        class="mr-4 required"
                                        :rules="[rules.required]"
                                        v-model="inputData.userName"
                                        clearable
                                    />
                                </v-col>
                            </v-row>
                            <v-row no-gutters >
                                <v-col>
                                    <v-text-field
                                        label="E-mail"
                                        class="mr-4 required"
                                        :rules="[rules.required, rules.email]"
                                        v-model="inputData.emailAddress"
                                        clearable
                                    />
                                </v-col>
                                <v-col>
                                    <v-select
                                        item-value="text"
                                        item-text="text"
                                        :items="perfis"
                                        :disabled="isTenant"
                                        v-model="perfil"
                                        :rules="[rules.required]"
                                        label="Perfil"
                                        class="mr-4 required"
                                        :menu-props="{ bottom: true, offsetY: true }"
                                        clearable
                                    />
                                </v-col>
                            </v-row>
                            <v-row no-gutters justify="space-between">
                                <v-col>
                                    <v-select
                                        v-if="perfil === 'Parceiro' "
                                        :disabled="isTenant"
                                        :items="parceiros"
                                        item-value="id"
                                        item-text="fantasia"
                                        v-model="parceiro"
                                        label="Parceiro"
                                        class="mr-4"
                                        :menu-props="{ bottom: true, offsetY: true }"
                                        clearable
                                    />
                                </v-col>
                                <v-col align="end" justify="end" class="justify-end">
                                    <v-row justify="end" class="pa-4 mr-4">
                                        <v-switch
                                            :label="inputData.isActive ? 'Ativo' : 'Inativo'"
                                            v-model="inputData.isActive"
                                        />
                                    </v-row>
                                </v-col>
                            </v-row>
                        </v-form>
                    </v-col>
                </v-row>
            </template>
        </alert-dialog>
    </v-container>

</template>
<script>
import {defineComponent} from 'vue'
import AlertDialog from "@/components/shared/Dialog.vue";
import {filtrarCaracterString} from "@/lib/helpers/validarCaracteresPermitidos";
import ValidationRules from "@/mixins/validationRules";
import {createNamespacedHelpers} from "vuex";
import ModalRedefinicaoSenha from "@/views/auth/modal/modalRedefinicaoSenha.vue";


const moduleUsuario = createNamespacedHelpers("ModuleUsuario");
const moduleParceiro = createNamespacedHelpers("ModuleParceiro");
const moduleRoles = createNamespacedHelpers("ModuleRoles");

export default defineComponent({
    name: "modalAddEditUsuario",
    components:{
        ModalRedefinicaoSenha,
        AlertDialog
    },
    props: {
        value : {type: Boolean, default: false},
        editId : {type: Number, default: null},
    },
    computed:{
        showDialog: {
            get() {
                return this.value;
            },
            set(newValue) {
                this.$emit("input", newValue);
            }
        },
        parceiro: {
            get() {
                return +this.inputData.tenantId
            },
            set(newValue) {
                this.inputData.tenantId = newValue;
            }
        }
    },
    data: () => ({
        inputData: {
            id: 0,
            name: null,
            userName: null,
            surname: null,
            tenantId: null,
            emailAddress: null,
            roleName: null,
            roleNames: null,
            isActive: true,
        },
        valid: true,
        perfil: "",
        parceiros: [],
        perfis: [],
        loading: false,
        isTenant: false,
        showModal: false,
        alertType: null,
        alertText: '',
        alertShow: false,
    }),
    methods: {
        ...moduleUsuario.mapActions(["create", "getById", "update" ]),
        ...moduleRoles.mapActions(["getAllRoles"]),
        ...moduleParceiro.mapActions(["getAllParceirosAtivos"]),
        filtrarCaracterString,

        async salvar(){
            this.loading = true;
            let response;

            if(this.editId){
                if (this.$refs.form.validate()) {
                    this.inputData.roleNames = [this.perfil]
                    response = await this.update(this.inputData);
                }else{
                    this.mensagemErro("Preencha os campos obrigatórios");
                }
            }else{
                if (this.$refs.form.validate()) {
                    this.inputData.roleName = this.perfil;
                    response = await this.create(this.inputData);
                }else{
                    this.mensagemErro("Preencha os campos obrigatórios");
                }
            }
            this.loading = false;
            if (response.status === 200){
                this.$emit("atualizaTabela");
                this.editId ?
                    this.$emit("mensagemSucesso", "Registro atualizado com sucesso!") :
                    this.$emit("mensagemSucesso", "Registro cadastrado com sucesso!")
                this.showDialog = false;
            } else {
                this.mensagemErro(response);
            }
        },

        async setInputData(){
            if (this.editId) {
                this.getById(this.editId).then((result) => {
                    if (result.status === 200) {
                        this.inputData = result.data.result;
                        this.perfis.forEach((e) => {
                            if(e.text.toLowerCase() === this.inputData.roleNames[0].toLowerCase()){
                                this.perfil = e.text;
                            }
                        });
                        this.parceiro = this.inputData.tenantId
                    }else{
                        this.mensagemErro("Ocorreu um erro ao realizar esta ação. Contate o administrador do sistema.");
                    }
                });
            }
            else {
                this.inputData = {
                    id: 0,
                    name: null,
                    userName: null,
                    surname: null,
                    tenantId: null,
                    emailAddress: null,
                    roleNames: null,
                    roleName: null,
                    perfil: "",
                    isActive: true,
                }
            }
        },

        async getParceiros(){
            this.parceiros = await this.getAllParceirosAtivos();
        },
        async getRoles(){
            this.perfis = await this.getAllRoles();
        },

        mensagemSucesso(mensagem){
            this.alertShow = true;
            this.alertType = "success";
            this.alertText = mensagem;
            setTimeout(() => {
                this.alertShow = false;
            }, 7000);
        },

        mensagemErro(mensagem){
            this.alertShow = true;
            this.alertType = "error";
            this.alertText = mensagem;
            setTimeout(() => {
                this.alertShow = false;
            }, 8000);
        },

    },
    mounted() {
        this.getParceiros();
        this.getRoles();
        if(this.$store.state.Session.tenant != null){
            this.isTenant = true;
            this.inputData.tenantId = this.$store.state.Session.tenant.id;
            this.inputData.roleName= "Parceiro";
            this.parceiro = this.inputData.tenantId;
        }
    },

    watch: {
        value() {
            this.setInputData();
        },
    },

    mixins: [
        ValidationRules
    ],
    
})
</script>



<style scoped>

</style>