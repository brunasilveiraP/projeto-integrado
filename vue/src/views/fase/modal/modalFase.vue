<template>
    <v-dialog v-model="value" persistent max-width="1000px">        
        <div class="white pa-8 d-flex flex-column">

            <div class="d-flex flex-row justify-space-between">
                <h2 class="bbl-neutral--text">{{!editId ? 'Nova Fase' : 'Edição Fase' }}</h2>
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
                        label="Descrição"
                        class="mr-4"
                        style="width: 30%"
                        v-model="inputData.descricao"
                        clearable
                    />

                    <v-checkbox
                        v-model="inputData.incluiCobranca"
                        label="Nesta fase, um projeto está apto para ser cobrado"
                        class="mr-4"
                        style="width: 20%"
                        clearable
                    >
                    </v-checkbox>

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

const moduleFase = createNamespacedHelpers("ModuleFase");

export default {
    
    props:[   
        "editId"
    ],
    
    components:{
    },

    name: 'modalFase',
    
    data: () => ({
        inputData: {
            id: 0,
            nome: null,
            descricao: null,
            incluiCobranca: false,
        },
        loading: false,
        alertErrorModal : false,
        alertErrorModalText: ''
    }),
    methods: {
        ...moduleFase.mapActions([ "getAll", "create", "getById", "update" ]),
        
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
                    descricao: null,
                    incluiCobranca: false,
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