import styles from './Button.module.css';

interface ButtonProps {
    src: string;
    alt: string;
    isActive: boolean;
    onClick: () => void;
}

const Button: React.FC<ButtonProps> = ({ src, alt, isActive, onClick }) => {
    return (
        <button
            type="button"
            onClick={onClick}
            className={`${styles.button} ${isActive ? `${styles.active}` : ''}`}
        >
            <img src={src} alt={alt} />
        </button>
    )
}

export default Button;