<template>
    <v-dialog v-model="value" persistent max-width="1000px">

        <div class="white pa-8 d-flex flex-column">

            <div class="d-flex flex-row justify-space-between">

                <div class="d-flex flex-row justify-space-between">
                    <h2 class="bbl-neutral--text">Selecione o parceiro</h2>
                </div>
                <div class="d-flex flex-row justify-space-between">
                    <v-btn text small fab @click="closeModal" class="hover-click">
                        <v-icon>mdi-close</v-icon>
                    </v-btn>
                </div>

            </div>

            <hr class="mb-3" />

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
                    <div class="d-flex" style="width: 100%">
                        <v-select
                            :items="parceiros"
                            item-value="id"
                            item-text="fantasia"
                            v-model="parceiro"
                            label="Parceiro"
                            :rules="[rules.required]"
                            class="mr-4 required"
                            style="width: 40%"
                            :menu-props="{ bottom: true, offsetY: true }"
                            clearable
                        ></v-select>
                    </div>

                </v-form>
            </div>

            <div style="text-align: right">
                <v-btn
                    class="bbl-orange white--text hover-click"
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
import { filtrarCaracterString } from "@/lib/helpers/validarCaracteresPermitidos.js";
import ValidationRules from "@/mixins/validationRules";

const moduleParceiro = createNamespacedHelpers("ModuleParceiro");
const moduleProjeto = createNamespacedHelpers("ModuleProjeto");

export default {

    props:{
        editId : Number,
        carregaParceiro: Boolean
    },

    components:{
    },

    name: 'modalParceiroSelect',

    data: () => ({
        inputData: {
            id: 0,
            tenantId: null,
        },
        parceiros: [],
        loading: false,
        alertErrorModal : false,
        alertErrorModalText: '',
        isTenant : false,
    }),
    methods: {
        ...moduleParceiro.mapActions(["getAllParceirosAtivos", "getByIdParceiro",]),
        ...moduleProjeto.mapActions(["create",]),

        filtrarCaracterString,

        async salvar(){
            if(this.$refs.form.validate()){
                this.loading = true;
                let response;
                response = await this.create(this.inputData);
                this.loading = false;
                if (response.status === 200){
                    this.closeModal();
                    this.routeLinkEditModal(response.data.result.id);
                } else {
                    this.alertErrorModal = true;
                    this.alertErrorModalText = response;
                    setTimeout(() => {
                        this.alertErrorModal = false;
                    }, 7000);
                }
            }
        },
        routeLinkEditModal(id) {
            this.$router.push({
                name: "ViewManutencaoProjetoEditar",
                params: { id: id },
            });
        },

        async getParceiros(){
            if(this.$store.state.Session.tenant != null){
                this.parceiros = [await this.getByIdParceiro(this.$store.state.Session.tenant.id)];
                this.parceiro = this.parceiros[0].id;
                this.isTenant = true;
            }else{
                this.parceiros = await this.getAllParceirosAtivos();
            }
        },

    },
    computed: {
        parceiro: {
            get() {
                return +this.inputData.tenantId
            },
            set(newValue) {
                this.inputData.tenantId = newValue;
            }
        }
    },
    mounted() {
        this.getParceiros();
    },

    watch: {
        
    },

    mixins: [
        ModalController, ValidationRules
    ],
};

</script>

<style scoped>

.rotate-icon {
    font-size: 5em;
    float: left;
    margin: 3px 20px;
    -webkit-transition: 0.6s ease-out;
    -moz-transition:  0.6s ease-out;
    transition:  0.6s ease-out;
}

.rotate-icon:hover{
    -webkit-transform: rotateZ(720deg);
    -moz-transform: rotateZ(720deg);
    transform: rotateZ(720deg);
}

</style>