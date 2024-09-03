<template>
  <div class="container">
    <a-spin :spinning="loading">
      <div class="form-container">
        <a-form @submit.prevent="onSubmit" layout="vertical">
          <a-form-item label="Name" required>
            <a-input v-model:value="form.name" placeholder="Enter your name" />
          </a-form-item>

          <a-form-item label="Surname" required>
            <a-input v-model:value="form.surname" placeholder="Enter your surname" />
          </a-form-item>

          <a-form-item label="Cellphone" required>
            <a-input v-model:value="form.cellphone" placeholder="Enter your cellphone" />
          </a-form-item>

          <a-form-item>
            <a-button type="primary" html-type="submit" block>Submit</a-button>
          </a-form-item>
        </a-form>
      </div>

      <a-table :columns="columns" :data-source="users" row-key="id">
    <template #bodyCell="{ column, record }">
      <template v-if="column.key === 'actions'">
        <a @click="editUser(record)">Edit</a>
        <a-divider type="vertical" />
        <a @click="deleteUser(record.id)" style="color: red;">Delete</a>
      </template>
    </template>
  </a-table>

      <a-modal v-model:visible="isModalVisible" title="Edit User" @ok="updateUser" @cancel="handleCancel">
        <a-form layout="vertical">
          <a-form-item label="Name">
            <a-input v-model:value="editForm.name" />
          </a-form-item>
          <a-form-item label="Surname">
            <a-input v-model:value="editForm.surname" />
          </a-form-item>
          <a-form-item label="Cellphone">
            <a-input v-model:value="editForm.cellphone" />
          </a-form-item>
        </a-form>
      </a-modal>
    </a-spin>
  </div>
</template>

<script setup>
import { ref ,onMounted} from 'vue';
import userService from './services/userService';
import { message } from 'ant-design-vue';

const form = ref({
  name: '',
  surname: '',
  cellphone: '',
});

const users = ref([]);
const loading = ref(false);
const isModalVisible = ref(false);
const editForm = ref({
  id: null,
  name: '',
  surname: '',
  cellphone: '',
});

const columns = [
  { title: 'Name', dataIndex: 'name', key: 'name' },
  { title: 'Surname', dataIndex: 'surname', key: 'surname' },
  { title: 'Cellphone', dataIndex: 'cellphone', key: 'cellphone' },
  {
    title: 'Actions',
    key: 'actions',
    scopedSlots: { customRender: 'actions' },
  },
];

const onSubmit = async () => {
  loading.value = true;
  try {
    await userService.createUserAsync(form.value);
    loadUsers();
    message.success('User added successfully');
    resetForm();
  } catch (error) {
    message.error('Failed to add user');
  } finally {
    loading.value = false;
  }
};

const editUser = (user) => {
  editForm.value = { ...user };
  isModalVisible.value = true;
};

const updateUser = async () => {
  loading.value = true;
  try {
     await  userService.updateUserAsync(editForm.value,editForm.value.id);
    loadUsers();
    message.success('User updated successfully');
    isModalVisible.value = false;
  } catch (error) {
    console.log(error)
    message.error('Failed to update user');
  } finally {
    loading.value = false;
  }
};

const deleteUser = async (id) => {
  loading.value = true;
  try {
    await userService.deleteUserAsync(id);
    users.value = users.value.filter(user => user.id !== id);
    message.success('User deleted successfully');
  } catch (error) {
    message.error('Failed to delete user');
  } finally {
    loading.value = false;
  }
};

const loadUsers = async () => {
  loading.value = true;
  try {
   const response = await userService.getUsersAsync();

    users.value = response.data.users;
  } catch (error) {
    console.error(error)
    message.error('Failed to load users');
  } finally {
    loading.value = false;

  }
};

const handleCancel = () => {
  isModalVisible.value = false;
};

const resetForm = () => {
  form.value = {
    name: '',
    surname: '',
    cellphone: '',
  };
};

onMounted(async () => {
   await loadUsers();
});
</script>

<style scoped>
.container {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  min-height: 100vh;
  padding: 20px;
  background-color: #f0f2f5;
}

.form-container {
  width: 100%;
  max-width: 400px;
  margin-bottom: 30px;
}

a-input {
  border-radius: 8px;
}

a-button {
  border-radius: 8px;
}
</style>
