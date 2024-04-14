<script lang="ts">
    import { enhance } from '$app/forms'
    export let data
</script>

<form method="POST" action="?/create" use:enhance>
    <label>
        Task title
        <input name="title" type="text" placeholder="Title" />
    </label>
    <label for="done">Done</label>
    <input id="done" name="done" type="checkbox" role="switch" placeholder="Done" />
    <input type="submit" value="Submit" />
</form>

<ul>
    {#each data.todos as todo}
        <li>
            <span>{todo.title}</span>
        </li>
    {/each}
</ul>
