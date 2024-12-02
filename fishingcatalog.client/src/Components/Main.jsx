import React, { useEffect, useState } from 'react'

function Main() {
    const [products, setProducts] = useState([])
    const [loading, setLoading] = useState(true)

    useEffect(() => {
        // ќтправл€ем запрос на сервер
        fetch('https://localhost:7040/Product')
            .then(response => {
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`)
                }
                return response.json()
            })
            .then(data => {
                setProducts(data)
                setLoading(false)
            })
            .catch(error => {
                console.error('Error fetching products:', error)
                setLoading(false)
            })
    }, []) // ѕустой массив зависимостей Ч запрос выполнитс€ только один раз при монтировании

    if (loading) {
        return <p>Loading...</p>
    }

    return (
        <div>
            <h1>Products</h1>
            {products.length > 0 ? (
                <ul>
                    {products.map(product => (
                        <li key={product.id}>
                            <h2>{product.name}</h2>
                            <p>Category: {product.category}</p>
                            <p>Price: ${product.price}</p>
                            <p>{product.description}</p>
                            
                        </li>
                    ))}
                </ul>
            ) : (
                <p>No products found.</p>
            )}
        </div>
    )
}


export default Main;