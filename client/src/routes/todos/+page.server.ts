import type { Todo } from '$lib'
import type { RequestEvent } from '@sveltejs/kit'

const unix = '/tmp/uds-dotnet-bun.sock'

export async function load() {
    const request = await fetch('http://localhost/todos', { unix })
    const todos: Todo[] = await request.json()
    return { todos }
}

export const actions = {
    create: async ({ request }: RequestEvent) => {
        const data = await request.formData()
        const formData = {
            title: data.get('title'),
            done: Boolean(data.get('done'))
        }

        await fetch('http://localhost/todos', {
            unix,
            method: 'POST',
            body: JSON.stringify(formData),
            headers: {
                'Content-Type': 'application/json',
            },
        })
    },

    toggle: async ({ request }: RequestEvent) => {
        const data = await request.formData()
        const id = data.get('id')
        const done = data.get('done') == 'false' // if false, then true -- toggled!
        return fetch(`http://localhost/todos/${id}`, {
            unix,
            method: 'DELETE',
            body: JSON.stringify({ done }),
            headers: {
                'Content-Type': 'application/json',
            },
        })
            .then(console.log)
            .catch(console.error)
    },
}
