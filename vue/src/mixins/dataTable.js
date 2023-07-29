import { createNamespacedHelpers } from 'vuex'
import { mapState } from "vuex";

export default ({ 
    store = '',
    module = createNamespacedHelpers(store) 
} = {}) => ({

    data: () => ({
        page: 1,
        totalItems: 0,
        options: {},
        aditionalData: null,
        tableLoading: false,
    }),

    computed: {
        ...module.mapState(['listItems']),
    },

    watch: {
        options: {
            handler() {
                this.loadItems(this.options, this.searchData, this.aditionalData);
            },
            deep: true,
        },
        searchData: {
            handler() {
                this.loadItems(this.options, this.searchData, this.aditionalData);
            },
            deep: true,
        },
        aditionalData:{
            handler(){
                this.loadItems(this.options, this.searchData, this.aditionalData);
            },
            deep: true
        }
    },

    methods: {

        ...module.mapActions(['getAll']),

        async loadItems(options, search, aditionalData) {
            let { page, itemsPerPage } = options;
            let pageNumber = page > 0 ? page - 1 : 0;
            try {
                this.tableLoading = true;
                await this.getAll({ pageNumber, itemsPerPage, search, aditionalData });
                this.totalItems = this.listItems.result.totalCount;
                this.numberOfPages = Math.ceil(this.listItems.result.totalCount / itemsPerPage);
                await this.buildRows(this.listItems.result.items);  
                this.tableLoading = false;
            } catch (error) {
                this.tableLoading = false;
                console.log(error);
            }
        },

    },

});

