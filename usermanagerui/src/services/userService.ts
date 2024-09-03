import apiClient from "./api";

export default {
    async createUserAsync(data: any) {
        return apiClient.post('api/User/CreateUser', data);
    },

    async updateUserAsync(data:any,id:number) {
        return apiClient.put(`api/User/EditUser/${id}`, data);
    },
    async deleteUserAsync(id: number) {
        await apiClient.delete(`api/User/DeleteUser/${id}`);
    },
     getUsersAsync() {
        return apiClient.get(`api/User/GetUsers`);
    }
}