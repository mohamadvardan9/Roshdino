{
  /*بخش اصلی و بزرگ بالای صفحه*/
}
{
  /*همان قسمتی که کاربر معمولا بلافاصله بعد از ورود به سایت تقی وردان بزرگ میبیند*/
}

import { motion } from "framer-motion";
import { fadeLeft, fadeRight } from "./animations/animations";
import Button from "./ui/Button";
import Container from "./ui/Container";
import heroImage from "../assets/heroMainLogo.png";

export default function Hero() {
  return (
    <section
      className="
            pt-40
            pb-24
            bg-gradient-to-b
            to-white
            "
    >
      <Container>
        <div
          className="
                grid
                grid-cols-1
                lg:grid-cols-2
                gap-10
                lg:gap-16
                items-center
            "
        >
          {/* Text Content */}

          <motion.div variants={fadeRight} initial="hidden" animate="visible">
            <h1
              className="order-1
                        text-4xl
                        sm:text-5xl
                        lg:text-6xl
                        font-black
                        leading-tight
                        text-center
                        lg:text-right
                        "
            >
              کسب‌وکار خود را
              <br />
              <span
                className="
                            text-purple-600
                            "
              >
                هوشمندانه رشد دهید
              </span>
            </h1>

            <p
              className="
                        mt-6
                        text-base
                        sm:text-lg
                        leading-8
                        text-gray-600
                        text-center
                        lg:text-right
                        "
            >
              ما به برندها کمک می‌کنیم با دیجیتال مارکتینگ، مشتری بیشتری جذب
              کنند
            </p>

            <div
              className="
                            mt-8
                            flex
                            flex-col
                            sm:flex-row
                            gap-4
                            justify-center
                            lg:justify-start
                        "
            >
              <Button>شروع همکاری</Button>

              <button
                className="
                            border
                            px-8
                            py-4
                            rounded-xl
                            "
              >
                نمونه کارها
              </button>
            </div>
          </motion.div>

          {/* Hero Image / Card */}

          <motion.div
            variants={fadeLeft}
            initial="hidden"
            animate="visible"
            className="
                    relative
                    "
          >
            <div
              className="
                    w-64
                    h-64
                    sm:w-100
                    sm:h-100
                    lg:w-96
                    lg:h-96
                    rounded-full
                    overflow-hidden
                    mx-auto
                    "
            >
              <img
                src={heroImage}
                alt="Roshdino"
                className="
                            w-full
                            h-full
                            object-contain
                            "
              />
            </div>
          </motion.div>
        </div>
      </Container>
    </section>
  );
}
