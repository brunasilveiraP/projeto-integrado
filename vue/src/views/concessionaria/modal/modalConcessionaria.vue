<template>
    <v-dialog v-model="value" persistent max-width="1000px">
        <div class="white pa-8 d-flex flex-column">

            <div class="d-flex flex-row justify-space-between">
                <h2 class="bbl-neutral--text">{{!editId ? 'Nova Concessionária' : 'Edição Concessionária' }}</h2>
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
                    <v-text-field
                        label="Nome"
                        class="mr-4 required"
                        style="width: 30%"
                        v-model="inputData.nome"
                        clearable
                    />

                    <v-text-field
                        label="E-mail"
                        class="mr-4"
                        style="width: 30%"
                        v-model="inputData.email"
                        clearable
                    />

                    <v-text-field
                        label="Telefone"
                        class="mr-4"
                        style="width: 30%"
                        v-model="inputData.telefone"
                        clearable
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

const moduleConcessionaria = createNamespacedHelpers("ModuleConcessionaria");

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
            nome: null,
            email: null,
            telefone: null,
        },
        loading: false,
        alertErrorModal : false,
        alertErrorModalText: ''
    }),
    methods: {
        ...moduleConcessionaria.mapActions([ "getAll", "create", "getById", "update" ]),
        
        async salvar(){
            this.loading = true;
            let response;
            
            if(this.editId){                
                response = await this.update(this.inputData);
            }else{
                response = await this.create(this.inputData);
            }
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

    watch: {
        value() {
            this.setInputData();       
        },        
    },

    mixins: [
        ModalController
    ],
};

</script>

<style scoped>

</style>