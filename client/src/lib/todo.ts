export type TodoDTO = {
    title: string,
    done: boolean
}

export type Todo = TodoDTO & {
    id: string
}
